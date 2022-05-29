using MetricsManager.Common;
using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Извлекает метрики из базы за указанный временной промежуток
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <param name="fromTime">Начало временного промежутка</param>
		/// <param name="toTime">Конец временного промежутка</param>
		/// <returns>Список метрик за указанный промежуток времени</returns>
		T GetByTimePeriod(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime);

		/// <summary>
		/// Извлекает метрики из базы за указанный временной промежуток
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <param name="fromTime">Начало временного промежутка</param>
		/// <param name="toTime">Конец временного промежутка</param>
		/// <param name="percentile">Перцентиль</param>
		/// <returns>Метрику за указанный промежуток времени для указанного перцентиля</returns>
		T GetByTimePeriodPercentile(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime, Percentile percentile);

		/// <summary>
		/// Ищет дату-время за которое был собрана последняя метрика для заданного агента
		/// </summary>
		/// <param name="agentId">Id агента</param>
		/// <returns>Дата-вреям последней собранной метрика для заданного агента</returns>
		DateTimeOffset GetLast(int agentId);

		/// <summary>
		/// Записывает значение метрики в базу данных
		/// </summary>
		/// <param name="metrics">Метрика для занесения в базу данных</param>
		void Create(T metrics);
	}
}
