using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using Dapper;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.MySQLSettings;
using MetricsManager.DAL.Repositories;
using MetricsManager.Common;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {
        /// <summary>
        /// Объект с именами и настройками базы данных
        /// </summary>
        private readonly IMySqlSettings mySql;
        private readonly ILogger _logger;

        public AgentsRepository(IMySqlSettings mySqlSettings, ILogger<AgentsRepository> logger)
        {
            mySql = mySqlSettings;
            _logger = logger;
        }

        public AgentInfo GetAgentInfoById(int agentId)
        {
            var agentInfo = new AgentInfo();
            using (var connection = new SQLiteConnection(mySql.ConnectionString))
            {
                try
                {
                    agentInfo = connection.QuerySingle<AgentInfo>(
                    "SELECT * " +
                    $"FROM {mySql.AgentsTable} " +
                    $"WHERE (" +
                    $"{mySql[Columns.AgentId]} == @agentId)",
                    new
                    {
                        agentId = agentId
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex.Message);
                }
            }

            return agentInfo;

        }

        public AllAgentsInfo GetAllAgentsInfo()
        {
            var agentsInfo = new AllAgentsInfo();
            using (var connection = new SQLiteConnection(mySql.ConnectionString))
            {
                try
                {
                    agentsInfo.Agents = connection.Query<AgentInfo>(
                    "SELECT * " +
                    $"FROM {mySql.AgentsTable} ").ToList();

                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex.Message);
                }

            }

            return agentsInfo;
        }

        public void RegisterAgent(AgentInfo agentInfo)
        {
            using (var connection = new SQLiteConnection(mySql.ConnectionString))
            {
                try
                {
                    connection.ExecuteAsync(
                    $"INSERT INTO {mySql.AgentsTable}" +
                    $"({mySql[Columns.AgentId]}, {mySql[Columns.AgentUri]})" +
                    $"VALUES (@agentId, @agentUri);",
                    new
                    {
                        agentId = agentInfo.AgentId,
                        agentUri = agentInfo.AgentUri,
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex.Message);
                }

            }
        }
    }
}
