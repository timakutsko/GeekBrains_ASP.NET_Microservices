using Metrics.Data;
using System.Collections.Generic;

namespace MetricsAgent.Models
{
    public class HddMetricsHolder
    {
        private List<HddMetric> _values;

        public HddMetricsHolder()
        {
            _values = new List<HddMetric>();
        }

        public void Add(HddMetric value)
        {
            _values.Add(value);
        }

        public HddMetric[] Get()
        {

            return _values.ToArray();
        }

        public List<HddMetric> Values
        {
            get { return _values; }
            set { _values = value; }
        }
    }
}
