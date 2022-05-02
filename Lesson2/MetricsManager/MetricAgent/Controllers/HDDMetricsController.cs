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
	/// Контроллер для обработки Hdd метрик
	/// </summary>
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HDDMetricsController : ControllerBase
    {
        private readonly ILogger<HDDMetricsController> _logger;
        private IHDDMetricsRepository _repository;
        private readonly IMapper _mapper;

        public HDDMetricsController(ILogger<HDDMetricsController> logger, IHDDMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("left/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] HDDMetricCreateRequest request)
        {
            // Логирование
            _logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
                $"Время с: {request.FromTime};\n" +
                $"Время по: {request.ToTime}.");

            // Основной процесс
            IList<HDDMetric> metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);
            var response = new HDDMetricsResponse()
            {
                Metrics = new List<HDDMetricDTO>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<HDDMetricDTO>(metric));
            }
            return Ok(response);
        }
    }
}
