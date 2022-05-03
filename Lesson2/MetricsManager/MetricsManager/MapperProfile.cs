using AutoMapper;
using MetricsManager.DAL.Models;
using MetricsManager.Responses;
using MetricsManager.Responses.FromManager;

namespace MetricsManager
{
	/// <summary>
	/// Профайлер для маппинга между моделями и DTO объектами метрик
	/// </summary>
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<AgentInfo, AgentInfoDTO>();
			CreateMap<CPUMetric, CPUMetricDTO>();
			CreateMap<HDDMetric, HDDMetricDTO>();
			CreateMap<NETMetric, NETMetricDTO>();
			CreateMap<NetworkMetric, NetworkMetricDTO>();
			CreateMap<RAMMetric, RAMMetricDTO>();
		}
	}
}
