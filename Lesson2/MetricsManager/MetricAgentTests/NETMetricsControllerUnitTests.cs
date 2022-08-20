using Castle.Core.Logging;
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
    public class NETMetricsControllerUnitTests
    {
        private NETMetricsController _controller;
        private Mock<ILogger<NETMetricsController>> _mockLogger;
        private Mock<INETMetricsRepository> _mockRepository;
        private NETMetricCreateRequest _request = new NETMetricCreateRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.Now
        };

        public NETMetricsControllerUnitTests()
        { 
            _mockLogger = new Mock<ILogger<NETMetricsController>>();
            _mockRepository = new Mock<INETMetricsRepository>();

            _controller = new NETMetricsController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void GetMetrics_ShouldCall_Create_From_Repository()
        {
            // Arrange

            // Act
            var result = _controller.GetMetrics(_request);

            // Assert
            _mockRepository.Verify(repository => repository.Create(It.IsAny<NETMetric>()), Times.AtMostOnce());
        }

        [Fact]
        public void GetMetrics_ShouldCall_GetByTimePeriod_From_Repository()
        {
            // Arrange

            // Act
            var result = _controller.GetMetrics(_request);

            // Assert
            _mockRepository.Verify(repository => repository.GetByTimePeriod(_request.FromTime, _request.ToTime), Times.AtMostOnce());
        }
    }
}