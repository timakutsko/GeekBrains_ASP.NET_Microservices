using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Models
{
	/// <summary>
	/// Контейнер для передачи списка с метриками
	/// </summary>
	public class AllMetrics<T>
	{
		public List<T> Metrics { get; set; }

		public AllMetrics()
		{
			Metrics = new List<T>();
		}
	}
}
