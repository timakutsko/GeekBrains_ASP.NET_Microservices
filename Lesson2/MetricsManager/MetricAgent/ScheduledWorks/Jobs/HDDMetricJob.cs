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
	/// Задача сбора Hdd метрик
	/// </summary>
	[DisallowConcurrentExecution]
	public class HDDMetricJob : IJob
	{
		// Инжектируем DI провайдер
		private readonly IServiceProvider _provider;
		private IHDDMetricsRepository _repository;

		/// <summary>Имя категории счетчика</summary>
		private readonly string categoryName = "LogicalDisk";
		/// <summary>Имя счетчика</summary>
		private readonly string counterName = "Free Megabytes";
		/// <summary>Счетчик</summary>
		private PerformanceCounter _counter;


		public HDDMetricJob(IServiceProvider provider)
		{
			_provider = provider;
			_repository = _provider.GetService<IHDDMetricsRepository>();
			_counter = new PerformanceCounter(categoryName, counterName, "_Total");
		}

		public Task Execute(IJobExecutionContext context)
		{
			// Складываем характеристики всех экземпляров счетчиков
			int value = Convert.ToInt32(_counter.NextValue());

			// Время когда была собрана метрика
			var time = DateTimeOffset.UtcNow;

			// Запись метрики в репозиторий
			_repository.Create(new HDDMetric { Time = time, Value = value });

			return Task.CompletedTask;
		}
	}
}