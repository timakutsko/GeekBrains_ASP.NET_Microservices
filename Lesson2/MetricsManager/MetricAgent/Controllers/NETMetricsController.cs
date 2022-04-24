using MetricAgent.DAL.Models;
using MetricAgent.DAL.Repositories;
using MetricAgent.Requests;
using MetricAgent.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MetricAgent.Controllers
{
    /// <summary>
    /// Контроллер для обработки DotNet метрик
    /// </summary>
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class NETMetricsController : ControllerBase
    {
        private readonly ILogger<NETMetricsController> _logger;
        private INETMetricsRepository _repository;

        public NETMetricsController(ILogger<NETMetricsController> logger, INETMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] MetricCreateRequest request)
        {
            _logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
                $"Время с: {request.FromTime};\n" +
                $"Время по: {request.ToTime}.");
            var metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);
            return Ok(metrics);
        }
    }
}
