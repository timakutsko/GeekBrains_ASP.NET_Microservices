using Metrics.Data;
using MetricsAgent.Services.Intefaces;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Services.Jobs
{
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;
        public CpuMetricJob(ICpuMetricsRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            float cpuUsageInPercents = _cpuCounter.NextValue();
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            Debug.WriteLine($"{time} > {cpuUsageInPercents}");
            _repository.Create(new CpuMetric
            {
                Ticks = time.Ticks,
                Value = cpuUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
