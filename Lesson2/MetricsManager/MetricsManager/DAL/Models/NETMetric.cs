using MetricsManager.DAL.Interfaces;
using System;

namespace MetricsManager.DAL.Models
{
    public class NETMetric : IMetricModel
    { 
        public int AgentId { get; set; } 
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
