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
	/// Контроллер для обработки Network метрик
	/// </summary>
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;
        private INetworkMetricsRepository _repository;
        private readonly IMapper _mapper;

        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug("Вызов конструктора");
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение Network метрик за заданный промежуток времени
        /// </summary>
        /// <param name="request">Запрос на выдачу метрик с интервалом времени</param>
        /// <returns>Список метрик за заданный интервал времени</returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] NetworkMetricCreateRequest request)
        {
            // Логирование
            _logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
                $"Время с: {request.FromTime};\n" +
                $"Время по: {request.ToTime}.");

            // Основной процесс
            IList<NetworkMetric> metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);
            var response = new NetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDTO>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<NetworkMetricDTO>(metric));
            }
            return Ok(response);
        }
    }
}

