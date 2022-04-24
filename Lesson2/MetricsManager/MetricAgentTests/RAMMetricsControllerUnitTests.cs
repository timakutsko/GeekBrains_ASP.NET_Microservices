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
    public class RANMetricsControllerUnitTests
    {
        private RAMMetricsController _controller;
        private Mock<ILogger<RAMMetricsController>> _mockLogger;
        private Mock<IRAMMetricsRepository> _mockRepository;
        private MetricCreateRequest _request = new MetricCreateRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.Now
        };

        public RANMetricsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<RAMMetricsController>>();
            _mockRepository = new Mock<IRAMMetricsRepository>();

            _controller = new RAMMetricsController(_mockLogger.Object, _mockRepository.Object);
        }

        [Fact]
        public void GetMetrics_ShouldCall_Create_From_Repository()
        {
            // Arrange

            // Act
            var result = _controller.GetMetrics(_request);

            // Assert
            _mockRepository.Verify(repository => repository.Create(It.IsAny<Metric>()), Times.AtMostOnce());
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