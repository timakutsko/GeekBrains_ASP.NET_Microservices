using System;

namespace MetricsManager.ScheduledWorks.Tools
{

	public class JobScheduleDTO
	{
		public JobScheduleDTO(Type jobType, string cronExpression)
		{
			JobType = jobType;
			CronExpression = cronExpression;
		}

		public Type JobType { get; }
		public string CronExpression { get; }
	}
}