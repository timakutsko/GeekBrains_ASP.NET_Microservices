using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using MetricAgent.DAL.Interfaces;
using MetricAgent.DAL.Models;
using Dapper;
using MetricAgent.MySQLSettings;

namespace MetricAgent.DAL.Repositories
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface ICPUMetricsRepository : IRepository<CPUMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки CPU метрик
	/// </summary>
	public class CPUMetricsRepository : ICPUMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings _mySql;

		public CPUMetricsRepository(IMySqlSettings mySqlSettings)
		{ 
			// Добавляем парсилку типа TimeSpan вкачестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
			_mySql = mySqlSettings;
		}

		public void Create(CPUMetric metric)
		{
			using (var connection = new SQLiteConnection(_mySql.ConnectionString))
			{
				//  Запрос на вставку данных с плейсхолдерами для параметров
				connection.ExecuteAsync(
				$"INSERT INTO {_mySql[Tables.CPUMetric]} " +
				$"({_mySql[Columns.Value]}, {_mySql[Columns.Time]})" +
				"VALUES(@value, @time)",
				// Анонимный объект с параметрами запроса
				new
				{
					// Value подставится на место "@value" в строке запроса 
					// Значение запишется из поля Value объекта item
					value = metric.Value,
					// Записываем в поле timeколичество секунд
					time = metric.Time.ToUnixTimeSeconds()
				});
			}
		}

		public IList<CPUMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
			using (var connection = new SQLiteConnection(_mySql.ConnectionString))
			{
				return connection.Query<CPUMetric>(
					$"SELECT * FROM {_mySql[Tables.CPUMetric]} " +
					$"WHERE ({_mySql[Columns.Time]} >= @fromTime AND {_mySql[Columns.Time]} <= @toTime)",
					new
					{
						fromTime = fromTime.ToUnixTimeSeconds(),
						toTime = toTime.ToUnixTimeSeconds()
					}).ToList();
			}
		}
    }
}