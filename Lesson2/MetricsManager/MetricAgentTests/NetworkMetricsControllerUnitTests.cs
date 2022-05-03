using AutoMapper;
using MetricAgent;
using MetricAgent.Controllers;
using MetricAgent.DAL.Models;
using MetricAgent.DAL.Repositories;
using MetricAgent.Requests;
using MetricAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
namespace MetricAgentTests
{
    public class NetworkMetricsControllerUnitTests
    {
        private NetworkMetricsController _controller;
        private Mock<ILogger<NetworkMetricsController>> _mockLogger;
        private Mock<INetworkMetricsRepository> _mockRepository;
        private NetworkMetricCreateRequest _request = new NetworkMetricCreateRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.UtcNow
        };
        private List<NetworkMetric> _mockMetrics = new List<NetworkMetric>()
        {
            { new NetworkMetric() {Time = DateTimeOffset.MinValue, Value = 100 } },
            { new NetworkMetric() {Time = DateTimeOffset.UtcNow, Value = 121 } }
        };

        public NetworkMetricsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<NetworkMetricsController>>();
            _mockRepository = new Mock<INetworkMetricsRepository>();

            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            _controller = new NetworkMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
        }

        [Fact]
        public void GetMetrics_ShouldCall_GetByTimePeriod_From_Repository()
        {
            // Arrange
            _mockRepository.
                Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
                Returns(_mockMetrics);

            // Act
            var result = _controller.GetMetrics(_request);

            // Assert
            _mockRepository.Verify(repository => repository.GetByTimePeriod(_request.FromTime, _request.ToTime), Times.AtMostOnce());
        }
        
        [Fact]
        public void GetMetricsByInterval_ReturnsCorrectMetrics()
        {
            // Arrange
            _mockRepository.
                Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).
                Returns(_mockMetrics);

            // Act
            var result = _controller.GetMetrics(_request);
            var response = ((result as OkObjectResult).Value as NetworkMetricsResponse).Metrics;

            bool check = true;
            if (_mockMetrics.Count == response.Count)
            {
                for (int i = 0; i < _mockMetrics.Count; i++)
                {
                    if ((_mockMetrics[i].Value != response[i].Value) ||
                        (_mockMetrics[i].Time != response[i].Time))
                    {
                        check = false;
                    }
                }
            }
            else
            {
                check = false;
            }

            // Assert
            Assert.True(check);
        }
    }
}