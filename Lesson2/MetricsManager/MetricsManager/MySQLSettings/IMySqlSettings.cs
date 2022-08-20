namespace MetricsManager.MySQLSettings
{
	/// <summary>
	/// Интерфейс для класса настроек базы данных
	/// </summary>
	public interface IMySqlSettings
	{
		/// <summary> Строка подключения к базе данных </summary>
		public string ConnectionString { get; }

		/// <summary> Название таблицы с данными об агентах </summary>
		public string AgentsTable { get; }

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
