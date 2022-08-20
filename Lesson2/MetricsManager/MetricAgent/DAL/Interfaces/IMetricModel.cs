using System;
using System.Collections.Generic;

namespace MetricAgent.DAL.Interfaces
{
    public interface IMetricModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
