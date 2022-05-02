using MetricAgent.DAL.Interfaces;
using System;

namespace MetricAgent.DAL.Models
{
    public class NetworkMetric : IMetricModel
    { 
        public int Id { get; set; } 
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
