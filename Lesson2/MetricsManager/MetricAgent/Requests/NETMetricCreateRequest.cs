using Microsoft.AspNetCore.Mvc;
using System;

namespace MetricAgent.Requests
{
	/// <summary>
	/// Контейнер для запроса метрик из базы
	/// </summary>
	public class NETMetricCreateRequest
	{
		[FromRoute]
		public DateTimeOffset FromTime { get; set; }
		
		[FromRoute]
		public DateTimeOffset ToTime { get; set; }
	} 
}
