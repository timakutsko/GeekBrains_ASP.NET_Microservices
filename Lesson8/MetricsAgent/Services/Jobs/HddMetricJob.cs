using Metrics.Data;
using MetricsAgent.Services.Intefaces;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Services.Jobs
{
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _hddCounter;
        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _hddCounter = new PerformanceCounter("LogicalDisk", "% Free Space", "_Total");

        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости Hdd
            float hddUsageInPercents = _hddCounter.NextValue();
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            Debug.WriteLine($"{time} > {hddUsageInPercents}");
            _repository.Create(new HddMetric
            {
                Ticks = time.Ticks,
                Value = hddUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}
