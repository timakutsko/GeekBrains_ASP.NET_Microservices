using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using MetricsManager.Common;
using System.Collections.Generic;
using System.Linq;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly AgentsContainer _agentsContainer;

        public AgentsController(AgentsContainer agentsContainer)
        {
            _agentsContainer = agentsContainer;
        }
        
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _agentsContainer.Agents.Add(agentInfo);
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById ([FromRoute] int agentId)
        {
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById ([FromRoute] int agentId)
        {
            _agentsContainer.Agents = _agentsContainer.Agents.Where(a => a.AgentId != agentId).ToList();
            return Ok();
        }

        [HttpGet("list")]
        public List<AgentInfo> RegistredAgents()
        {
            return _agentsContainer.Agents;
        }
    }
}
