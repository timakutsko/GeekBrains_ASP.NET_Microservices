using MetricAgent.DAL.Models;
using MetricAgent.Requests;
using MetricAgent.Responses;
using MetricAgent.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using AutoMapper;

namespace MetricAgent.Controllers
{
    /// <summary>
    /// Контроллер для обработки Cpu метрик
    /// </summary>
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CPUMetricsController : ControllerBase
    {
        private readonly ILogger<CPUMetricsController> _logger;
        private ICPUMetricsRepository _repository;
        private readonly IMapper _mapper;

        public CPUMetricsController(ILogger<CPUMetricsController> logger, ICPUMetricsRepository repository)
        {
            _logger = logger;
            _repository = repository; 
        }
        
        /// <summary>
        /// Получение CPU метрик за заданный промежуток времени
        /// </summary>
        /// <param name="request">Запрос на выдачу метрик с интервалом времени</param>
        /// <returns>Список метрик за заданный интервал времени</returns>
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] CPUMetricCreateRequest request)
        {
            // Логирование
            _logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
                $"Время с: {request.FromTime};\n" +
                $"Время по: {request.ToTime}.");
            
            // Основной процесс
            IList<CPUMetric> metrics = _repository.GetByTimePeriod(request.FromTime, request.ToTime);
            var response = new CPUMetricsResponse() 
            { 
                Metrics = new List<CPUMetricDTO>() 
            }; 
            foreach (var metric in metrics) 
            { 
                response.Metrics.Add(_mapper.Map<CPUMetricDTO>(metric)); 
            }
            return Ok(response);
        }
    }
}
