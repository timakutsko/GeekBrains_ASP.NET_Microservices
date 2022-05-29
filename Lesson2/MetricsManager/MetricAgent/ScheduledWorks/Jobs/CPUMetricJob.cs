using MetricAgent.DAL.Models;
using MetricAgent.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.ScheduledWorks.Jobs
{
	/// <summary>
	/// Задача сбора Cpu метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class CPUMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private readonly ICPUMetricsRepository _repository;

		/// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "Processor";
		/// <summary>Имя счетчика</summary>
		private readonly string counterName = "% Processor Time";
		/// <summary>Счетчик</summary>
		private PerformanceCounter _counter;


		public CPUMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			_repository = _provider.GetService<ICPUMetricsRepository>();
			_counter = new PerformanceCounter(categoryName, counterName, "_Total");
		}

		public Task Execute(IJobExecutionContext context)
		{
			// Складываем характеристики всех экземпляров счетчиков
			int value = Convert.ToInt32(_counter.NextValue());

			// Время когда была собрана метрика
			var time = DateTimeOffset.UtcNow;

			// Запись метрики в репозиторий
			_repository.Create(new CPUMetric { Time = time, Value = value });

			return Task.CompletedTask;
		}
	}
}