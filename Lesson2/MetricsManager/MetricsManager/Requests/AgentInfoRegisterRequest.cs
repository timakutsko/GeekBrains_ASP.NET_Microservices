using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Requests
{
	/// <summary>
	/// Контейнер с данными для регистрации агента
	/// </summary>
	public class AgentInfoRegisterRequest
	{
		/// <summary>
		/// Id агента
		/// </summary>
		public int AgentId { get; set; }

		/// <summary>
		/// Адрес агента
		/// </summary>
		public string AgentUri { get; set; }
	}
}
