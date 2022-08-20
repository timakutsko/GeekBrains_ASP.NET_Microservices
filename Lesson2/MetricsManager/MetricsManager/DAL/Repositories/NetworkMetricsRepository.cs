using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.MySQLSettings;
using Dapper;
using Microsoft.Extensions.Logging;
using MetricsManager.Common;

namespace MetricsManager.DAL.Repositories
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface INetworkMetricsRepository : IRepository<AllMetrics<NetworkMetric>>
	{
	}

	/// <summary>
	/// Репозиторий для обработки Network метрик
	/// </summary>
	public class NetworkMetricsRepository : INetworkMetricsRepository
	{
		/// <summary>
		/// Объект с именами и настройками базы данных
		/// </summary>
		private readonly IMySqlSettings mySql;
		private readonly ILogger _logger;

		public NetworkMetricsRepository(IMySqlSettings mySqlSettings, ILogger<NetworkMetricsRepository> logger)
		{
			// Добавляем парсилку типа DateTimeOffset в качестве подсказки для SQLite
			SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
			mySql = mySqlSettings;
			_logger = logger;
		}

		/// <summary>
		/// Записывает метрику в базу данных
		/// </summary>
		/// <param name="metrics">Список метрик для записи</param>
		public void Create(AllMetrics<NetworkMetric> metrics)
		{
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					try
					{
						foreach (var metric in metrics.Metrics)
						{
							connection.ExecuteAsync(
							$"INSERT INTO {mySql[Tables.NetworkMetric]}" +
							$"({mySql[Columns.AgentId]}, {mySql[Columns.Value]}, {mySql[Columns.Time]})" +
							$"VALUES (@agentid, @value, @time);",
							new
							{
								value = metric.Value,
								time = metric.Time.ToUnixTimeSeconds(),
								agentId = metric.AgentId,
							});
						}
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						_logger.LogDebug(ex.Message);
					}

				}

			}
		}

		/// <summary>
		/// Возвращает список с метриками за заданный интервал времени
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <param name="fromTime">Начало временного интервала</param>
		/// <param name="toTime">Конец временного интервала</param>
		/// <returns>Список с метриками за заданный интервал времени</returns>
		public AllMetrics<NetworkMetric> GetByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			var metrics = new AllMetrics<NetworkMetric>();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				try
				{
					metrics.Metrics = connection.Query<NetworkMetric>(
					"SELECT * " +
					$"FROM {mySql[Tables.NetworkMetric]} " +
					$"WHERE (" +
					$"{mySql[Columns.AgentId]} == @agentId) " +
					$"AND {mySql[Columns.Time]} >= @fromTime " +
					$"AND {mySql[Columns.Time]} <= @toTime ",
					new
					{
						agentId = agentId,
						fromTime = fromTime.ToUnixTimeSeconds(),
						toTime = toTime.ToUnixTimeSeconds(),
					}).ToList();
				}
				catch (Exception ex)
				{
					_logger.LogDebug(ex.Message);
				}
				return metrics;
			}
		}

		public AllMetrics<NetworkMetric> GetByTimePeriodPercentile(
			int agentId,
			DateTimeOffset fromTime,
			DateTimeOffset toTime,
			Percentile percentile)
		{
			var metrics = new AllMetrics<NetworkMetric>();
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				try
				{
					metrics.Metrics = connection.Query<NetworkMetric>(
					"SELECT * " +
					$"FROM {mySql[Tables.NetworkMetric]} " +
					$"WHERE (" +
					$"{mySql[Columns.AgentId]} == @agentId) " +
					$"AND {mySql[Columns.Time]} >= @fromTime " +
					$"AND {mySql[Columns.Time]} <= @toTime " +
					$"ORDER BY {mySql[Columns.Value]}",
					new
					{
						agentId = agentId,
						fromTime = fromTime.ToUnixTimeSeconds(),
						toTime = toTime.ToUnixTimeSeconds(),
					}).ToList();
				}
				catch (Exception ex)
				{
					_logger.LogDebug(ex.Message);
				}
			}

			var percentileIndex = ((int)percentile * metrics.Metrics.Count / 100);

			var returnMetrics = new AllMetrics<NetworkMetric>();
			returnMetrics.Metrics.Add(metrics.Metrics[percentileIndex - 1]);

			return returnMetrics;
		}


		public DateTimeOffset GetLast(int agentId)
		{
			DateTimeOffset lastTime = DateTimeOffset.FromUnixTimeSeconds(0);
			using (var connection = new SQLiteConnection(mySql.ConnectionString))
			{
				try
				{
					lastTime = connection.QueryFirst<DateTimeOffset>(
						$"SELECT MAX({mySql[Columns.Time]}) " +
						$"FROM {mySql[Tables.NetworkMetric]} " +
						$"WHERE {mySql[Columns.AgentId]} == @agentId",
						new
						{
							agentId = agentId
						});
				}
				catch (Exception ex)
				{
					_logger.LogDebug(ex.Message);
				}

				return lastTime;
			}
		}

	}
}