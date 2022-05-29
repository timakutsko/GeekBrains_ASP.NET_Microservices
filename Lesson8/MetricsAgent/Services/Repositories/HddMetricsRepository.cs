using Metrics.Data;
using MetricsAgent.Models;
using MetricsAgent.Services.Intefaces;

namespace MetricsAgent.Services
{
    public class HddMetricsRepository : IHddMetricsRepository
    {

        private readonly HddMetricsHolder _hddMetricsHolder;

        public HddMetricsRepository(HddMetricsHolder hddMetricsHolder)
        {
            _hddMetricsHolder = hddMetricsHolder;
        }

        public void Create(HddMetric metric)
        {
            _hddMetricsHolder.Add(metric);
        }

        public HddMetric[] Get()
        {
            return _hddMetricsHolder.Get();
        }
    }
}
