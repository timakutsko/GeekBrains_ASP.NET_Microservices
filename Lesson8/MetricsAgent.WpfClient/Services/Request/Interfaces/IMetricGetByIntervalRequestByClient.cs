using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsAgent.WpfClient.Services.Request.Interfaces
{
	public interface IMetricGetByIntervalRequestByClient
	{
		public string ClientBaseAddress { get; set; }
		public TimeSpan FromTime { get; set; }
		public TimeSpan ToTime { get; set; }
	}
}
