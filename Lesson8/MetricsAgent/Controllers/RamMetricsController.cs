using MetricsAgent.Services;
using MetricsAgent.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {

        private readonly IRamMetricsRepository _ramMetricsRepository;

        public RamMetricsController(IRamMetricsRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster(
            [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            return Ok(_ramMetricsRepository.Get());
        }
    }
}
