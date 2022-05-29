using Metrics.Data;
using System.Collections.Generic;

namespace MetricsAgent.Models
{
    public class RamMetricsHolder
    {
        private List<RamMetric> _values;

        public RamMetricsHolder()
        {
            _values = new List<RamMetric>();
        }

        public void Add(RamMetric value)
        {
            _values.Add(value);
        }

        public RamMetric[] Get()
        {

            return _values.ToArray();
        }

        public List<RamMetric> Values
        {
            get { return _values; }
            set { _values = value; }
        }
    }
}
