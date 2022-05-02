namespace MetricAgent.MySQLSettings
{
	public interface IMySqlSettings
	{
		/// <summary> Строка подключения к базе данных </summary>
		public string ConnectionString { get; }

		/// <summary> Индексатор для имен таблиц </summary>
		/// <param name="key">Ключ для имени таблицы</param>
		/// <returns>Имя таблицы по ключу</returns>
		public string this[Tables key] { get; }

		/// <summary> Индексатор для имен рядов </summary>
		/// <param name="key">Ключ для имени ряда</param>
		/// <returns>Имя ряда по ключу</returns>
		public string this[Columns key] { get; }
	}
}
