using Metrics.Data;
using System.Collections.Generic;

namespace MetricsAgent.Models
{
    public class CpuMetricsHolder
    {
        private List<CpuMetric> _values;

        public CpuMetricsHolder()
        {
            _values = new List<CpuMetric>();
        }

        public void Add(CpuMetric value)
        {
            _values.Add(value);
        }

        public CpuMetric[] Get()
        {

            return _values.ToArray();
        }

        public List<CpuMetric> Values
        {
            get { return _values; }
            set { _values = value; }
        }
    }
}
