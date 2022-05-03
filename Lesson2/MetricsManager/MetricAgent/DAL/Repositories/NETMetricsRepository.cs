using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using MetricAgent.DAL.Interfaces;
using MetricAgent.DAL.Models;
using MetricAgent.MySQLSettings;
using Dapper;

namespace MetricAgent.DAL.Repositories
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface INETMetricsRepository : IRepository<NETMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки .NET метрик
	/// </summary>
	public class NETMetricsRepository : INETMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings _mySql;

		public NETMetricsRepository(IMySqlSettings mySqlSettings)
		{
			// Добавляем парсилку типа TimeSpan вкачестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
			_mySql = mySqlSettings;
		}

		public void Create(NETMetric metric)
		{
			using (var connection = new SQLiteConnection(_mySql.ConnectionString))
			{
				//  Запрос на вставку данных с плейсхолдерами для параметров
				connection.ExecuteAsync(
				$"INSERT INTO {_mySql[Tables.NETMetric]} " +
				$"({_mySql[Columns.Value]}, {_mySql[Columns.Time]})" +
				"VALUES(@value, @time)",
				// Анонимный объект с параметрамизапроса
				new
				{
					// Value подставится на место"@value" в строке запроса 
					// Значение запишется из поля Value объекта item
					value = metric.Value,
					// Записываем в поле timeколичество секунд
					time = metric.Time.ToUnixTimeSeconds()
				});
			}
		}

		public IList<NETMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			using (var connection = new SQLiteConnection(_mySql.ConnectionString))
			{
				return connection.Query<NETMetric>(
					$"SELECT * FROM {_mySql[Tables.NETMetric]} " +
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