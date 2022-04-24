using System;
using System.Collections.Generic;

namespace MetricAgent.Responses
{
	/// <summary>
	/// Контейнер для передачи списка метрик в ответе от сервера
	/// </summary>
	public class AllMetricsResponse
	{
		public List<MetricDto> Metrics { get; set; }
	}

	/// <summary>S
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class MetricDto
	{
		public DateTimeOffset Time { get; set; }
		public int Value { get; set; }
		public int Id { get; set; }
	}
}