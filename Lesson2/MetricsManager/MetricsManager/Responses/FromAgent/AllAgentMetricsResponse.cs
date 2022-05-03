using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Responses.FromAgent
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllAgentMetricsResponse<T>
	{
		public List<T> Metrics { get; set; }

		public AllAgentMetricsResponse()
		{
			Metrics = new List<T>();
		}

	}
}
