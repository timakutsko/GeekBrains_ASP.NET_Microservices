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
    public class HDDMetricsControllerUnitTests
    {
        private HDDMetricsController _controller;
        private Mock<ILogger<HDDMetricsController>> _mockLogger;
        private Mock<IHDDMetricsRepository> _mockRepository;
        private MetricCreateRequest _request = new MetricCreateRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.Now
        };

        public HDDMetricsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<HDDMetricsController>>();
            _mockRepository = new Mock<IHDDMetricsRepository>();

            _controller = new HDDMetricsController(_mockLogger.Object, _mockRepository.Object);
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