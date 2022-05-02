using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class HDDMetricsResponse
	{
		public List<HDDMetricDTO> Metrics { get; set; }
	}

	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class HDDMetricDTO
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
	}
}