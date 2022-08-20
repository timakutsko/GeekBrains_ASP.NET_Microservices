using MetricsManager.DAL.Models;
using System;
using System.Collections.Generic;

namespace MetricsManager.DAL.Interfaces
{
	public interface IAgentsRepository
	{
		/// <summary>
		/// Возвращает список с данными по всем зарегистрированным агентам
		/// </summary>
		/// <returns>Список с данными по всем зарегистрированным агентам</returns>
		AllAgentsInfo GetAllAgentsInfo();

		/// <summary>
		/// Возвращает данные агента с указанным Id
		/// </summary>
		/// <param name="agentId">Id агента данные которого необходимо получить</param>
		/// <returns>Lанные агента с указанным Id</returns>
		AgentInfo GetAgentInfoById(int agentId);

		/// <summary>
		/// Регистрация агента в базе данных
		/// </summary>
		/// <param name="agentInfo">Информация по регистрируемому агенту</param>
		void RegisterAgent(AgentInfo agentInfo);
	}
}
