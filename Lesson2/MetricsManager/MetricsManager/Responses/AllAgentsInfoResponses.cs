using System;
using System.Collections.Generic;

namespace MetricsManager.Responses
{
	/// <summary>
	/// Контейнер для передачи списка с информацие об агентах
	/// </summary>
	public class AllAgentsInfoResponse
	{
		public List<AgentInfoDTO> Agents { get; set; }

		public AllAgentsInfoResponse()
		{
			Agents = new List<AgentInfoDTO>();
		}

	}

	/// <summary>
	/// Контейнер для передачи информации об агенте
	/// </summary>
	public class AgentInfoDTO
	{
		public int AgentId { get; set; }
		public string AgentUri { get; set; }
	}
}