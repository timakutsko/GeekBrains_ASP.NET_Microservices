using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
	{
		private AgentsController _controller;
		private Mock<ILogger<AgentsController>> _mockLogger;
		private Mock<IAgentsRepository> _mockAgentsRepository;

		public AgentsControllerUnitTests()
		{
			_mockLogger = new Mock<ILogger<AgentsController>>();
			_mockAgentsRepository = new Mock<IAgentsRepository>();
			var myProfile = new MapperProfile();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
			var mapper = new Mapper(configuration);

			_controller = new AgentsController(_mockLogger.Object, _mockAgentsRepository.Object, mapper);

		}

		[Fact]
		public void Read_ReturnsCorrectAgentsInfo()
		{
			//Arrange
			//фейковые данные об агентах
			var mockAgentsInfo = new AllAgentsInfo();
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 1, AgentUri = "url1" });
			mockAgentsInfo.Agents.Add(new AgentInfo() { AgentId = 2, AgentUri = "url2" });

			_mockAgentsRepository.
				Setup(repository => repository.GetAllAgentsInfo()).
				Returns(mockAgentsInfo);

			//Act
			var result = _controller.Read();

			var responseMetrics = ((result as OkObjectResult).Value as AllAgentsInfoResponse);

			bool check = true;
			if (mockAgentsInfo.Agents.Count == responseMetrics.Agents.Count)
			{
				for (int i = 0; i < mockAgentsInfo.Agents.Count; i++)
				{
					if ((mockAgentsInfo.Agents[i].AgentId != responseMetrics.Agents[i].AgentId) ||
						(mockAgentsInfo.Agents[i].AgentUri != responseMetrics.Agents[i].AgentUri))
					{
						check = false;//Если хоть одоин элемент в любой паре метрик не совпадает - проверка провалена
					}
				}
			}
			else//Если длина контейнеров не совпадает - проверка провалена
			{
				check = false;
			}

			// Assert
			Assert.True(check);
		}

		[Fact]
		public void RegistrAgent_ReturnsOk()
		{
			//Arrange
			var request = new AgentInfoRegisterRequest() { AgentId = 101, AgentUri = "http://AgentAdressUri" };

			//Act
			var result = _controller.RegisterAgent(request);

			// Assert
			Assert.IsAssignableFrom<IActionResult>(result);
		}

	}
}
