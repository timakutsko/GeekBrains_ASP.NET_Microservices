using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class RAMMetricsControllerUnitTests
    {
        private RAMMetrricsController controller;

        public RAMMetricsControllerUnitTests()
        {
            controller = new RAMMetrricsController();
        }

        [Fact]
        public void GetLeftHDDMemory_ReturnsOk()
        {

            //Arrange

            //Act
            var result = controller.GetLeftRAMMemory();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
