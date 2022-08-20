using Metrics.Data;
using MetricsAgent.Models;

namespace MetricsAgent.Services.Intefaces
{
    public interface ICpuMetricsRepository
    {
        void Create(CpuMetric metric);
        CpuMetric[] Get();
    }
}
