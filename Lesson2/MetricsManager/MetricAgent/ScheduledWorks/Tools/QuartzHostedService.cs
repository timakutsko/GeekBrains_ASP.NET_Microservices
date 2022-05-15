using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsAgent.ScheduledWorks.Tools
{

	public class QuartzHostedService : IHostedService
	{
		private readonly ISchedulerFactory _schedulerFactory;
		private readonly IJobFactory _jobFactory;
		private readonly IEnumerable<JobScheduleDTO> _jobSchedules;
		public IScheduler Scheduler { get; set; }

		public QuartzHostedService(
			ISchedulerFactory schedulerFactory,
			IJobFactory jobFactory,
			IEnumerable<JobScheduleDTO> jobSchedules)
		{
			_schedulerFactory = schedulerFactory;
			_jobSchedules = jobSchedules;
			_jobFactory = jobFactory;
		}

		private static IJobDetail CreateJobDetail(JobScheduleDTO schedule)
		{
			var jobType = schedule.JobType;
			return JobBuilder
				.Create(jobType)
				.WithIdentity(jobType.FullName)
				.WithDescription(jobType.Name)
				.Build();
		}

		private static ITrigger CreateTrigger(JobScheduleDTO schedule)
		{
			return TriggerBuilder
				.Create()
				.WithIdentity($"{schedule.JobType.FullName}.trigger")
				.WithCronSchedule(schedule.CronExpression)
				.WithDescription(schedule.CronExpression)
				.Build();
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
			Scheduler.JobFactory = _jobFactory;

			foreach (var jobSchedule in _jobSchedules)
			{
				var job = CreateJobDetail(jobSchedule);
				var trigger = CreateTrigger(jobSchedule);

				await Scheduler.ScheduleJob(job, trigger, cancellationToken);
			}

			await Scheduler.Start(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			await Scheduler?.Shutdown(cancellationToken);
		}
	}
}