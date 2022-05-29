using Metrics.Data;
using MetricsAgent.Models;

namespace MetricsAgent.Services.Intefaces
{
    public interface IRamMetricsRepository
    {
        void Create(RamMetric metric);
        RamMetric[] Get();
    }
}
