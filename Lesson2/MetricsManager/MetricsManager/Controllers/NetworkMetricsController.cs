﻿using AutoMapper;
using MetricsManager.Common;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Repositories;
using MetricsManager.Requests;
using MetricsManager.Requests.Interfaces;
using MetricsManager.Responses.FromManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
	{
		private readonly ILogger<NetworkMetricsController> _logger;
		private readonly INetworkMetricsRepository _repository;
		private readonly IAgentsRepository _agentRepository;
		private readonly IMapper _mapper;

		public NetworkMetricsController(
			ILogger<NetworkMetricsController> logger,
			INetworkMetricsRepository repository,
			IAgentsRepository agentRepository,
			IMapper mapper)
		{
			_logger = logger;
			_logger.LogDebug("Вызов конструктора");
			_repository = repository;
			_mapper = mapper;
			_agentRepository = agentRepository;
		}

		/// <summary>
		/// Запрос списка Network метрик от определенного агента за промежуток времени
		/// </summary>
		/// <param name="request">Id агента и временной интервал</param>
		/// <returns>Список Network метрик</returns>
		[HttpGet("agent/{AgentId}/from/{FromTime}/to/{ToTime}")]
		public IActionResult GetMetricsFromAgent([FromRoute] NetworkMetricGetByIntervalForAgentRequest request)
		{
			// Логирование
			_logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
				$"Агент id: {request.AgentId};\n" +
				$"Время с: {request.FromTime};\n" +
				$"Время по: {request.ToTime}.");

			// Основной процесс
			var metrics = _repository.GetByTimePeriod(request.AgentId, request.FromTime, request.ToTime);
			var response = new AllMetricsResponse<NetworkMetricDTO>();
			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<NetworkMetricDTO>(metric));
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос перцентиля Network метрик от определенного агента за промежуток времени
		/// </summary>
		/// <param name="request">Id агента и временной интервал</param>
		/// <param name="percentile">Перцентиль</param>
		/// <returns>Перцентиль Network метрик</returns>
		[HttpGet("agent/{AgentId}/from/{FromTime}/to/{ToTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAgent(
			[FromRoute] NetworkMetricGetByIntervalForAgentRequest request,
			[FromRoute] Percentile percentile)
		{
			// Логирование
			_logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
				$"Агент id: {request.AgentId};\n" +
				$"Время с: {request.FromTime};\n" +
				$"Время по: {request.ToTime};\n" +
				$"Выбранный персентиль: {percentile}.");

			// Основной процесс
			var metrics = _repository.GetByTimePeriodPercentile(request.AgentId, request.FromTime, request.ToTime, percentile);
			var response = new AllMetricsResponse<NetworkMetricDTO>();
			foreach (var metric in metrics.Metrics)
			{
				response.Metrics.Add(_mapper.Map<NetworkMetricDTO>(metric));
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос списка Network метрик от всех агентов за промежуток времени
		/// </summary>
		/// <param name="request">Временной интервал</param>
		/// <returns>Список Network метрик</returns>
		[HttpGet("cluster/from/{FromTime}/to/{ToTime}")]
		public IActionResult GetMetricsFromAllCluster([FromRoute] NetworkMetricGetByIntervalForClusterRequest request)
		{
			// Логирование
			_logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
				$"Время с: {request.FromTime};\n" +
				$"Время по: {request.ToTime}.");

			// Основной процесс
			var agents = _agentRepository.GetAllAgentsInfo();
			var response = new AllMetricsResponse<NetworkMetricDTO>();
			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimePeriod(agent.AgentId, request.FromTime, request.ToTime);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<NetworkMetricDTO>(metric));
				}
			}

			return Ok(response);
		}

		/// <summary>
		/// Запрос перцентиля Network метрик для каждого агента за промежуток времени
		/// </summary>
		/// <param name="request">Временной интервал</param>
		/// <param name="percentile">Перцентиль</param>
		/// <returns>Перцентиль Network метрик</returns>
		[HttpGet("cluster/from/{FromTime}/to/{ToTime}/percentiles/{percentile}")]
		public IActionResult GetMetricsByPercentileFromAllCluster(
			[FromRoute] NetworkMetricGetByIntervalForClusterRequest request,
			[FromRoute] Percentile percentile)
		{

			// Логирование
			_logger.LogInformation($"\nМетод {MethodBase.GetCurrentMethod().Name}. Пользовательский ввод:\n" +
				$"Время с: {request.FromTime};\n" +
				$"Время по: {request.ToTime};\n" +
				$"Выбранный персентиль: {percentile}.");

			// Основной процесс
			var agents = _agentRepository.GetAllAgentsInfo();
			var response = new AllMetricsResponse<NetworkMetricDTO>();
			foreach (var agent in agents.Agents)
			{
				var currentAgentMetrics = _repository.GetByTimePeriodPercentile(agent.AgentId, request.FromTime, request.ToTime, percentile);

				foreach (var metric in currentAgentMetrics.Metrics)
				{
					response.Metrics.Add(_mapper.Map<NetworkMetricDTO>(metric));
				}

			}

			return Ok(response);
		}
	}
}
