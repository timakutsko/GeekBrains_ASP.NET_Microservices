using MetricAgent.DAL.Models;
using MetricAgent.DAL.Repositories;
using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.ScheduledWorks.Jobs
{
	/// <summary>
	/// Задача сбора Ram метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class RAMMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private IRAMMetricsRepository _repository;

		/// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Memory";
		/// <summary>Имя счетчика</summary>
		private readonly string counterName = "Available MBytes";
		/// <summary>Счетчик</summary>
		private PerformanceCounter _counter;


		public RAMMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			_repository = _provider.GetService<IRAMMetricsRepository>();
			_counter = new PerformanceCounter(categoryName, counterName);
		}

		public Task Execute(IJobExecutionContext context)
		{
			// Складываем характеристики всех экземпляров счетчиков
			int value = Convert.ToInt32(_counter.NextValue());

			// Время когда была собрана метрика
			var time = DateTimeOffset.UtcNow;

			// Запись метрики в репозиторий
			_repository.Create(new RAMMetric { Time = time, Value = value });

			return Task.CompletedTask;
		}
	}
}