using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class NETMetricsController : ControllerBase
    {
        
        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetAllDotnetError([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok();
        }
    }
}
