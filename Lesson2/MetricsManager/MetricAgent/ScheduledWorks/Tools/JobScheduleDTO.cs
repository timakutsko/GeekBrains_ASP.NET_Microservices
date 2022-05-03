using System;

namespace MetricsAgent.ScheduledWorks.Tools
{

	public class JobScheduleDTO
	{
		public Type JobType { get; }
		public string CronExpression { get; }

		public JobScheduleDTO(Type jobType, string cronExpression)
		{
			JobType = jobType;
			CronExpression = cronExpression;
		}


	}
}