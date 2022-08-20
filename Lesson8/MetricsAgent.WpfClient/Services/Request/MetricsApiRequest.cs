using MetricsAgent.WpfClient.Services.Request.Interfaces;
using System;

namespace MetricsAgent.WpfClient.Services.Request
{
    public class MetricsApiRequest : IMetricGetByIntervalRequestByClient
    {
        public string ClientBaseAddress { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
