using Metrics.Data;
using MetricsAgent.Services.Intefaces;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Services.Jobs
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _ramCounter;
        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");

        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости RAM
            float ramUsageInPercents = _ramCounter.NextValue();
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            Debug.WriteLine($"{time} > {ramUsageInPercents}");
            _repository.Create(new RamMetric
            {
                Ticks = time.Ticks,
                Value = ramUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
