using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using MetricAgent.DAL.Interfaces;
using MetricAgent.DAL.Models;

namespace MetricAgent.DAL.Repositories
{
	/// <summary>
	/// Маркировочный интерфейс. Необходим, чтобы проверить работу репозитория на тесте-заглушке 
	/// </summary>
	public interface IRAMMetricsRepository : IRepository<Metric>
	{
	}

	/// <summary>
	/// Репозиторий для обработки RAM метрик
	/// </summary>
	public class RAMMetricsRepository : IRAMMetricsRepository
	{
		private const string _connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
		//Инжектируем соединение с базой данных в наш репозиторий через конструктор
		public void Create(Metric metric)
		{
			using var connection = new SQLiteConnection(_connectionString);
			connection.Open();
			// Создаём команду
			using var cmd = new SQLiteCommand(connection);
			// Прописываем в команду SQL-запрос на вставку данных
			cmd.CommandText = "INSERT INTO rammetrics (value,time) VALUES (@value, @time)";
			// Добавляем параметры в запрос из нашего объекта
			cmd.Parameters.AddWithValue("@value", metric.Value);
			// В таблице будем хранить время в секундах,поэтому преобразуем перед записью в секунды через свойство
			cmd.Parameters.AddWithValue("@time", metric.Time.ToUnixTimeSeconds());
			// Подготовка команды к выполнению
			cmd.Prepare();
			// Выполнение команды
			cmd.ExecuteNonQuery();
		}

		public IList<Metric> GetByTimePeriod(DateTimeOffset fromTime, DateTimeOffset toTime)
		{
			using var connection = new SQLiteConnection(_connectionString);
			connection.Open();

			using var cmd = new SQLiteCommand(connection);
			cmd.CommandText = "SELECT * FROM rammetrics WHERE ({mySql[Columns.Time]} >= @fromTime AND {mySql[Columns.Time]} <= @toTime)";
			cmd.Parameters.AddWithValue("@fromTime", fromTime);
			cmd.Parameters.AddWithValue("@toTime", toTime);

			var returnList = new List<Metric>();
			using (SQLiteDataReader reader = cmd.ExecuteReader())
			{
				// Пока есть что читать — читаем
				while (reader.Read())
				{ // Добавляем объект в список возврата
					returnList.Add(new Metric
					{
						Id = reader.GetInt32(0),
						Value = reader.GetInt32(1),
						//Налету преобразуем прочитанные секунды в метку времени
						Time = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32(2))
					});
				}
			}
			return returnList;
		}
	}
}