using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class CPUMetricsResponse
	{
		public List<CPUMetricDTO> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class CPUMetricDTO
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}
}