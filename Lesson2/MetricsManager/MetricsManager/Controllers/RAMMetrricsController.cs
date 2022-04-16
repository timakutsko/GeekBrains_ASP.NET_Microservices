using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RAMMetrricsController : ControllerBase
    {
        
        [HttpGet("available")]
        public IActionResult GetLeftRAMMemory()
        {
            return Ok();
        }

    }
}
