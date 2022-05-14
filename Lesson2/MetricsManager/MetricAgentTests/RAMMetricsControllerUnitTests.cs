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
    public class RAMMetricsControllerUnitTests
    {
        private RAMMetricsController _controller;
        private Mock<ILogger<RAMMetricsController>> _mockLogger;
        private Mock<IRAMMetricsRepository> _mockRepository;
        private RAMMetricCreateRequest _request = new RAMMetricCreateRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.UtcNow
        };
        private List<RAMMetric> _mockMetrics = new List<RAMMetric>()
        {
            { new RAMMetric() {Time = DateTimeOffset.MinValue, Value = 100 } },
            { new RAMMetric() {Time = DateTimeOffset.UtcNow, Value = 121 } }
        };

        public RAMMetricsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<RAMMetricsController>>();
            _mockRepository = new Mock<IRAMMetricsRepository>();

            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            _controller = new RAMMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
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
            var response = ((result as OkObjectResult).Value as RAMMetricsResponse).Metrics;

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