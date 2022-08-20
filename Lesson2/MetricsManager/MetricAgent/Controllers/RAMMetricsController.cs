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
	/// Контроллер для обработки Ram метрик
	/// </summary>
    [Route("api/metrics/ram")]
    [ApiController]
    public class RAMMetricsController : ControllerBase
    {
        private readonly ILogger<RAMMetricsController> _logger;
        private IRAMMetricsRepository _repository;
        private readonly IMapper _mapper;

        public RAMMetricsController(ILogger<RAMMetricsController> logger, IRAMMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("available/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] RAMMetricCreateRequest request)
        {
            // Логирование
            _logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
                $"Время с: {request.FromTime};\n" +
                $"Время по: {request.ToTime}.");

            // Основной процесс
            IList<RAMMetric> metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);
            var response = new RAMMetricsResponse()
            {
                Metrics = new List<RAMMetricDTO>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RAMMetricDTO>(metric));
            }
            return Ok(response);
        }
    }
}

