using Metrics.Data;
using MetricsAgent.Models;
using MetricsAgent.Services.Intefaces;

namespace MetricsAgent.Services
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {

        private readonly CpuMetricsHolder _cpuMetricsHolder;

        public CpuMetricsRepository(CpuMetricsHolder cpuMetricsHolder)
        {
            _cpuMetricsHolder = cpuMetricsHolder;
        }

        public void Create(CpuMetric metric)
        {
            _cpuMetricsHolder.Add(metric);
        }

        public CpuMetric[] Get()
        {
            return _cpuMetricsHolder.Get();
        }
    }
}
