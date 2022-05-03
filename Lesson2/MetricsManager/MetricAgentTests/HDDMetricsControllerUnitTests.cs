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
    public class HDDMetricsControllerUnitTests
    {
        private HDDMetricsController _controller;
        private Mock<ILogger<HDDMetricsController>> _mockLogger;
        private Mock<IHDDMetricsRepository> _mockRepository;
        private HDDMetricCreateRequest _request = new HDDMetricCreateRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.UtcNow
        };
        private List<HDDMetric> _mockMetrics = new List<HDDMetric>()
        {
            { new HDDMetric() {Time = DateTimeOffset.MinValue, Value = 100 } },
            { new HDDMetric() {Time = DateTimeOffset.UtcNow, Value = 121 } }
        };

        public HDDMetricsControllerUnitTests()
        {
            _mockLogger = new Mock<ILogger<HDDMetricsController>>();
            _mockRepository = new Mock<IHDDMetricsRepository>();

            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            _controller = new HDDMetricsController(_mockLogger.Object, _mockRepository.Object, mapper);
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
            var response = ((result as OkObjectResult).Value as HDDMetricsResponse).Metrics;

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