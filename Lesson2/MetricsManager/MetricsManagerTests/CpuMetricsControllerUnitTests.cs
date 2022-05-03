using AutoMapper;
using MetricsManager;
using MetricsManager.Common;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using MetricsManager.Requests;
using MetricsManager.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
namespace MetricsManagerTests
{
    public class CPUMetricsControllerUnitTests
    {
        private CPUMetricsController _controller;
        private Mock<ILogger<CPUMetricsController>> _mockLogger;
        private Mock<ICPUMetricsRepository> _mockMetricRepository;
        private Mock<IAgentsRepository> _mockAgentRepository;
        private CPUMetricGetByIntervalForClusterRequest _requestGetByIntervalForCluster = new CPUMetricGetByIntervalForClusterRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.UtcNow
        };
        private CPUMetricGetByIntervalForAgentRequest _requestGetByIntervalForAgentCluster = new CPUMetricGetByIntervalForAgentRequest()
        {
            AgentId = 1,
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.UtcNow
        };
        private CPUMetricGetByIntervalRequestByClient _requestGetByIntervalRequestByClient = new CPUMetricGetByIntervalRequestByClient()
        {
            AgentUri = "testUri",
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.UtcNow
        };
        private AllMetrics<CPUMetric> _mockMetrics = new AllMetrics<CPUMetric>();
        private readonly Percentile _percentile = Percentile.P99;
        private AllAgentsInfo _allAgentsInfo = new AllAgentsInfo();

        public CPUMetricsControllerUnitTests()
        {
            var mockMetrics = new AllMetrics<CPUMetric>();
            mockMetrics.Metrics.Add(new CPUMetric() { AgentId = 1, Time = DateTimeOffset.MinValue, Value = 100 });
            mockMetrics.Metrics.Add(new CPUMetric() { AgentId = 1, Time = DateTimeOffset.UtcNow, Value = 121 });
            _mockMetrics = mockMetrics;

            _mockLogger = new Mock<ILogger<CPUMetricsController>>();
            _mockMetricRepository = new Mock<ICPUMetricsRepository>();
            _mockAgentRepository = new Mock<IAgentsRepository>();

            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            _controller = new CPUMetricsController(
                _mockLogger.Object, 
                _mockMetricRepository.Object, 
                _mockAgentRepository.Object, 
                mapper);
        }

        [Fact]
        public void GetMetricsFromAgent_ShouldCall_GetByTimePeriod_From_MetricRepository()
        {
            // Arrange
            _mockMetricRepository.
                Setup(repository => repository.GetByTimePeriod(
                    It.IsAny<int>(), 
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>())).
                Returns(_mockMetrics);

            // Act
            var result = _controller.GetMetricsFromAgent(_requestGetByIntervalForAgentCluster);

            // Assert
            _mockMetricRepository.Verify(repository => repository.GetByTimePeriod(
                _requestGetByIntervalForAgentCluster.AgentId,
                _requestGetByIntervalForAgentCluster.FromTime,
                _requestGetByIntervalForAgentCluster.ToTime
                ), Times.AtMostOnce());
        }

        [Fact]
        public void GetMetricsByPercentileFromAgent_ShouldCall_GetByTimePeriodPercentile_From_MetricRepository()
        {
            // Arrange
            _mockMetricRepository.
                Setup(repository => repository.GetByTimePeriodPercentile(
                    It.IsAny<int>(), 
                    It.IsAny<DateTimeOffset>(), 
                    It.IsAny<DateTimeOffset>(),
                    _percentile)).
                Returns(_mockMetrics);

            // Act
            var result = _controller.GetMetricsByPercentileFromAgent(_requestGetByIntervalForAgentCluster, _percentile);

            // Assert
            _mockMetricRepository.Verify(repository => repository.GetByTimePeriodPercentile(
                _requestGetByIntervalForAgentCluster.AgentId,
                _requestGetByIntervalForAgentCluster.FromTime,
                _requestGetByIntervalForAgentCluster.ToTime,
                _percentile
                ), Times.AtMostOnce());
        }

        [Fact]
        public void GetMetricsFromAllCluster_ShouldCall_GetAllAgentsInfo_From_AgentRepository()
        {
            // Arrange
            _mockAgentRepository.
                Setup(repository => repository.GetAllAgentsInfo()).Returns(_allAgentsInfo);

            // Act
            var result = _controller.GetMetricsFromAllCluster(_requestGetByIntervalForCluster);

            // Assert
            _mockAgentRepository.Verify(repository => repository.GetAllAgentsInfo(), Times.AtMostOnce());
        }

        [Fact]
        public void GetMetricsByPercentileFromAllCluster_ShouldCall_GetAllAgentsInfo_From_AgentRepository()
        {
            // Arrange
            _mockAgentRepository.
                Setup(repository => repository.GetAllAgentsInfo()).Returns(_allAgentsInfo);

            // Act
            var result = _controller.GetMetricsByPercentileFromAllCluster(_requestGetByIntervalForCluster, _percentile);

            // Assert
            _mockAgentRepository.Verify(repository => repository.GetAllAgentsInfo(), Times.AtMostOnce());
        }
    }
}