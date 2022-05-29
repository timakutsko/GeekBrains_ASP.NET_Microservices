using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.WpfClient.Services.Response
{
	/// <summary>
	/// Контейнер для передачи метрики в ответе от сервера
	/// </summary>
	public class MetricResponse
	{
		public DateTimeOffset Time { get; set; }
		public float Value { get; set; }
	}
}
