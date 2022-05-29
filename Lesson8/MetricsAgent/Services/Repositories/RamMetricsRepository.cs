using Metrics.Data;
using MetricsAgent.Models;
using MetricsAgent.Services.Intefaces;

namespace MetricsAgent.Services
{
    public class RamMetricsRepository : IRamMetricsRepository
    {

        private readonly RamMetricsHolder _ramMetricsHolder;

        public RamMetricsRepository(RamMetricsHolder ramMetricsHolder)
        {
            _ramMetricsHolder = ramMetricsHolder;
        }

        public void Create(RamMetric metric)
        {
            _ramMetricsHolder.Add(metric);
        }

        public RamMetric[] Get()
        {
            return _ramMetricsHolder.Get();
        }
    }
}
