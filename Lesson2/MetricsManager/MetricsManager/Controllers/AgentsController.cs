using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using MetricsManager.Common;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly AgentsContainer _agentsContainer;
        private readonly ILogger<AgentsController> _logger;

        public AgentsController(AgentsContainer agentsContainer, ILogger<AgentsController> logger)
        {
            _logger = logger;
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

        [HttpGet("read")]
        public List<AgentInfo> ReadAgents()
        {
            return _agentsContainer.Agents;
        }
    }
}
