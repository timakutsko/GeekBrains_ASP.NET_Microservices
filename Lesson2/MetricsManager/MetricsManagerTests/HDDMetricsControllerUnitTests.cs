using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class HDDMetricsControllerUnitTests
    {
        private HDDMetricsController controller;

        public HDDMetricsControllerUnitTests()
        {
            controller = new HDDMetricsController();
        }

        [Fact]
        public void GetLeftHDDMemory_ReturnsOk()
        {

            //Arrange

            //Act
            var result = controller.GetLeftHDDMemory();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
