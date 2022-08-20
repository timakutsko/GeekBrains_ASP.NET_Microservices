using MetricAgent.DAL.Interfaces;
using System;

namespace MetricAgent.DAL.Models
{
    public class NETMetric
    { 
        public int Id { get; set; } 
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
