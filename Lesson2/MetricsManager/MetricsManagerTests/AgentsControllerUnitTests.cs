using MetricsManager.Common;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController controller;
        private readonly ILogger<AgentsController> _logger;

        public AgentsControllerUnitTests() 
        {
            controller = new AgentsController(new AgentsContainer(), _logger); 
        }
     
        [Fact]
        public void RegisterAgent_ReturnsOk()
        {

            //Arrange
            AgentInfo agentInfo = new AgentInfo();
            
            //Act
            IActionResult result = controller.RegisterAgent(agentInfo);
            
            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgentById_ReturnsOk()
        {

            //Arrange
            int agentId = 1;

            //Act
            IActionResult result = controller.EnableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_ReturnsOk()
        {

            //Arrange
            int agentId = 1;

            //Act
            IActionResult result = controller.DisableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
