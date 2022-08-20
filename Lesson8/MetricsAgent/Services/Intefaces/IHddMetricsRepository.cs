using Metrics.Data;
using MetricsAgent.Models;

namespace MetricsAgent.Services.Intefaces
{
    public interface IHddMetricsRepository
    {
        void Create(HddMetric metric);
        HddMetric[] Get();
    }
}
