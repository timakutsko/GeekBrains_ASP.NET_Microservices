using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
    public interface IMetricModel
    {
        public int AgentId { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
