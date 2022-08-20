using System.Collections.Generic;

namespace MetricAgent.MySQLSettings
{
	/// <summary> Ключи имен таблиц </summary>
	public enum Tables
	{
		CPUMetric,
		HDDMetric,
		NETMetric,
		NetworkMetric,
		RAMMetric,
	}
	
	/// <summary> Ключи имен рядов </summary>
	public enum Columns
	{
		Id,
		Value,
		Time,
	}

	/// <summary>Класс для хранения настроек базы данных</summary>
	public class MySqlSettings : IMySqlSettings
	{
		/// <summary>Словарь для хранения имен таблиц</summary>
		private readonly Dictionary<Tables, string> tablesNames = new Dictionary<Tables, string>
		{
			{Tables.CPUMetric, "cpumetrics" },
			{Tables.HDDMetric, "hddmetrics" },
			{Tables.NETMetric, "dotnetmetrics" },
			{Tables.NetworkMetric, "networkmetrics" },
			{Tables.RAMMetric, "rammetrics" },
		};

		/// <summary>Словарь для хранения имен рядов в таблицах</summary>
		private readonly Dictionary<Columns, string> rowsNames = new Dictionary<Columns, string>
		{
			{Columns.Id, "id" },
			{Columns.Value, "value" },
			{Columns.Time, "time" },
		};

		/// <summary> Строка для подключения к базе данных </summary>
		private readonly string _connectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
		}

		public string this[Tables key]
		{
			get
			{
				return tablesNames[key];
			}
		}

		public string this[Columns key]
		{
			get
			{
				return rowsNames[key];
			}
		}
	}
}
