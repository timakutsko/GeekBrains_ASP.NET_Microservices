using MetricAgent.DAL.Models;
using MetricAgent.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.ScheduledWorks.Jobs
{
	/// <summary>
	/// Задача сбора Network метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class NetworkMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private INetworkMetricsRepository _repository;

		/// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Network Interface";
		/// <summary>Имя счетчика</summary>
		private readonly string counterName = "Bytes Received/sec";
		/// <summary>Список для хранения всех экземпляров счетчика</summary>
		private List<PerformanceCounter> _counters = new List<PerformanceCounter>();


		public NetworkMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			_repository = _provider.GetService<INetworkMetricsRepository>();
			// Получаем массив с именами всех экземпляров для подсчета и формируем список счетчиков
			string[] instances = new PerformanceCounterCategory(categoryName).GetInstanceNames();
			foreach (var instance in instances)
			{
				_counters.Add(new PerformanceCounter(categoryName, counterName, instance));
			}

		}

		public Task Execute(IJobExecutionContext context)
		{
			// Складываем характеристики всех экземпляров счетчиков
			int value = 0;
			foreach (var counter in _counters)
			{
				value += Convert.ToInt32(counter.NextValue());
			}

			// Время когда была собрана метрика
			var time = DateTimeOffset.UtcNow;

			// Запись метрики в репозиторий
			_repository.Create(new NetworkMetric { Time = time, Value = value });

			return Task.CompletedTask;
		}
	}
}