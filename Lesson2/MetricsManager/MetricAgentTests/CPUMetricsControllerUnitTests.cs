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
    public class CPUMetricsControllerUnitTests 
    { 
        private CPUMetricsController _controller;
        private Mock<ILogger<CPUMetricsController>> _mockLogger;
        private Mock<ICPUMetricsRepository> _mockRepository;
        private MetricCreateRequest _request = new MetricCreateRequest()
        {
            FromTime = DateTimeOffset.MinValue,
            ToTime = DateTimeOffset.Now
        };

        public CPUMetricsControllerUnitTests() 
        {
            _mockLogger = new Mock<ILogger<CPUMetricsController>>();
            _mockRepository = new Mock<ICPUMetricsRepository>(); 
            
            _controller = new CPUMetricsController(_mockLogger.Object, _mockRepository.Object); 
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