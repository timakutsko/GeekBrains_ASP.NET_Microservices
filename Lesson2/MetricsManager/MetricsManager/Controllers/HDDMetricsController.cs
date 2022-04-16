using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HDDMetricsController : ControllerBase
    {
        
        [HttpGet("left")]
        public IActionResult GetLeftHDDMemory()
        {
            return Ok();
        }
    }
}
