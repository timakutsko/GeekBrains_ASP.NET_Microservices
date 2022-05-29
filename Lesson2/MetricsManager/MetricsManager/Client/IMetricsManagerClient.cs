using MetricsManager.Requests;
using MetricsManager.Requests.Interfaces;
using MetricsManager.Responses;
using MetricsManager.Responses.FromAgent;

namespace MetricsManager.Client
{
	public interface IMetricsManagerClient
	{
		AllAgentMetricsResponse<T> GetMetrics<T>(IMetricGetByIntervalRequestByClient request, ApiNames apiName);

	}
}
