using System;

namespace MetricAgent.Requests
{
    public interface IMetricCreateRequest
    {
        DateTimeOffset FromTime { get; set; }

        DateTimeOffset ToTime { get; set; }
    }
}
