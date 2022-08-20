using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using MetricsManager.Responses.FromAgent;
using MetricsManager.Requests.Interfaces;

namespace MetricsManager.Client
{
	public enum ApiNames
	{
		Cpu,
		DotNet,
		Hdd,
		Network,
		Ram,
	}

	public class MetricsManagerClient : IMetricsManagerClient
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger _logger;

		private readonly Dictionary<ApiNames, string> apiNames = new Dictionary<ApiNames, string>()
		{
			{ApiNames.Cpu, "cpu" },
			{ApiNames.DotNet, "dotnet" },
			{ApiNames.Hdd, "hdd" },
			{ApiNames.Network, "network" },
			{ApiNames.Ram, "ram" },
		};


		public MetricsManagerClient(HttpClient httpClient, ILogger<MetricsManagerClient> logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		public AllAgentMetricsResponse<T> GetMetrics<T>(IMetricGetByIntervalRequestByClient request, ApiNames apiName)
		{
			var fromParameter = request.FromTime.ToString("O");
			var toParameter = request.ToTime.ToString("O");
			var httpRequest = new HttpRequestMessage(
				HttpMethod.Get,
				$"{request.AgentUri}/api/metrics/{apiNames[apiName]}/from/{fromParameter}/to/{toParameter}");

			try
			{
				HttpResponseMessage response = _httpClient.SendAsync(httpRequest).Result;

				using var responseStream = response.Content.ReadAsStreamAsync().Result;
				using var streamReader = new StreamReader(responseStream);
				var content = streamReader.ReadToEnd();

				var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
				var returnResp = JsonSerializer.Deserialize<AllAgentMetricsResponse<T>>(content, options);
				return returnResp;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return null;
		}

	}
}
