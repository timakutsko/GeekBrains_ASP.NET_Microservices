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
	public interface IHDDMetricsRepository : IRepository<HDDMetric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки HDD метрик
	/// </summary>
	public class HDDMetricsRepository : IHDDMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings _mySql;

		public HDDMetricsRepository(IMySqlSettings mySqlSettings)
		{
			// Добавляем парсилку типа TimeSpan вкачестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
			_mySql = mySqlSettings;
		}

		public void Create(HDDMetric metric)
		{
			using (var connection = new SQLiteConnection(_mySql.ConnectionString))
			{
				//  Запрос на вставку данных с плейсхолдерами для параметров
				connection.ExecuteAsync(
				$"INSERT INTO {_mySql[Tables.HDDMetric]} " +
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

		public IList<HDDMetric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			using (var connection = new SQLiteConnection(_mySql.ConnectionString))
			{
				return connection.Query<HDDMetric>(
					$"SELECT * FROM {_mySql[Tables.HDDMetric]} " +
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