using AutoMapper;
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
        private readonly IMapper _mapper;

        public NETMetricsController(ILogger<NETMetricsController> logger, INETMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug("Вызов конструктора");
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("errors-count/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] NETMetricCreateRequest request)
        {
            // Логирование
            _logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
                $"Время с: {request.FromTime};\n" +
                $"Время по: {request.ToTime}.");

            // Основной процесс
            IList<NETMetric> metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);
            var response = new NETMetricsResponse()
            {
                Metrics = new List<NETMetricDTO>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NETMetricDTO>(metric));
            }
            return Ok(response);
        }
    }
}
