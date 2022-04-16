using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class NETMetricsControllerUnitTests
    {
        private NETMetricsController controller;

        public NETMetricsControllerUnitTests()
        {
            controller = new NETMetricsController();
        }

        [Fact]
        public void GetLeftHDDMemory_ReturnsOk()
        {

            //Arrange
            TimeSpan fromTime = TimeSpan.FromSeconds(0);
            TimeSpan toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = controller.GetAllDotnetError(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
