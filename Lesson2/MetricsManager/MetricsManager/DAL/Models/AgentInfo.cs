using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Models
{

	public class AllAgentsInfo
	{
		public List<AgentInfo> Agents { get; set; }

		public AllAgentsInfo()
		{
			Agents = new List<AgentInfo>();
		}
	}

	public class AgentInfo
	{
		public int AgentId { get; set; }

		public string AgentUri { get; set; }

	}
}
