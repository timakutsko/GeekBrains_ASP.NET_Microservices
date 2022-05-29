using MetricsManager.DAL;
using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using MetricsManager.Client;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL.Repositories;
using MetricsManager.DAL.Interfaces;
using MetricsManager.Responses.FromAgent;
using MetricsManager.DAL.Models;

namespace MetricsManager.ScheduledWorks.Jobs
{
	/// <summary>
	/// Задача сбора RAM метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class RAMMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private IRAMMetricsRepository _repository;
		private IAgentsRepository _agentsRepository;
		private IMapper _mapper;
		private IMetricsManagerClient _client;
		private readonly ILogger _logger;


		public RAMMetricJob(IServiceProvider provider, IMapper mapper, IMetricsManagerClient client, ILogger<RAMMetricJob> logger)
		{
			_provider = provider;
			_repository = _provider.GetService<IRAMMetricsRepository>();
			_agentsRepository = _provider.GetService<IAgentsRepository>();
			_mapper = mapper;
			_client = client;
			_logger = logger;
		}

		public Task Execute(IJobExecutionContext context)
		{
			_logger.LogDebug("== RamMetricJob START - " +
				$"Time {DateTimeOffset.UtcNow}");
			//Получаем из репозитория агентов список всех агентов
			var allAgentsInfo = _agentsRepository.GetAllAgentsInfo();

			//Обрабатываем каждого агента в списке
			foreach (var agentInfo in allAgentsInfo.Agents)
			{
				//Временная метка, когда для текущего агента была снята последняя метрика
				var lastTime = _repository.GetLast(agentInfo.AgentId);

				// Создаем запрос для получения от текущего агента метрик за период времени
				// от последней проверки до текущего момента
				var request = new RAMMetricGetByIntervalRequestByClient()
				{
					AgentUri = agentInfo.AgentUri,
					FromTime = lastTime,
					ToTime = DateTimeOffset.UtcNow,
				};

				// Делаем запрос к Агенту метрик и получаем список метрик
				var response = _client.GetMetrics<RAMMetricFromAgentDTO>(request, ApiNames.RAM);

				if (response != null)
				{
					// Убираем из выборки первую метрику если она совпадает с последней сохраненной в базе
					// для исключения дублирования данных в базе
					if (response.Metrics[0].Time == lastTime)
					{
						response.Metrics.RemoveAt(0);
					}

					// Перекладываем данные из Response в модели метрик
					var recievedMetrics = new AllMetrics<RAMMetric>();
					foreach (var metricDto in response.Metrics)
					{
						recievedMetrics.Metrics.Add(new RAMMetric
						{
							AgentId = agentInfo.AgentId,
							Time = metricDto.Time,
							Value = metricDto.Value
						});
					}
					_repository.Create(recievedMetrics);
				}

			}
			_logger.LogDebug("!= RamMetricJob END - " +
				$"Time {DateTimeOffset.UtcNow}");
			return Task.CompletedTask;
		}
	}
}