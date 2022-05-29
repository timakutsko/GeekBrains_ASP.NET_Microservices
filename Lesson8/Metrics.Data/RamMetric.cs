using Metrics.Data.Interfaces;
using Newtonsoft.Json;
using System;

namespace Metrics.Data
{
    public class RamMetric : IMetric
    {
        public float Value { get; set; }

        public long Ticks { get; set; }
    }
}
