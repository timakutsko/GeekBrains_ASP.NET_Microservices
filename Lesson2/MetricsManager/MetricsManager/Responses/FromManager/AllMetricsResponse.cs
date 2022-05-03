using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses.FromManager
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllMetricsResponse<T>
	{
		public List<T> Metrics { get; set; }

		public AllMetricsResponse()
		{
			Metrics = new List<T>();
		}
	}
}
