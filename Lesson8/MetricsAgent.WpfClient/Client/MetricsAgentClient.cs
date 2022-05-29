using MetricsAgent.WpfClient.Services.Request.Interfaces;
using MetricsAgent.WpfClient.Services.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetricsAgent.WpfClient.Services.Client
{
    public enum ApiNames
    {
        CPU,
        NET,
        HDD,
        Network,
        RAM,
    }

    public class MetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<ApiNames, string> _apiNames = new Dictionary<ApiNames, string>()
        {
            {ApiNames.CPU, "cpu" },
            {ApiNames.NET, "dotnet" },
            {ApiNames.HDD, "hdd" },
            {ApiNames.Network, "network" },
            {ApiNames.RAM, "ram" },
        };

        public MetricsAgentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<MetricResponse>> GetMetrics(IMetricGetByIntervalRequestByClient request, ApiNames apiName)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{request.ClientBaseAddress}/api/metrics/{_apiNames[apiName]}/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage httpResponseMessage = _httpClient.SendAsync(httpRequest).Result;
                string response = await httpResponseMessage.Content.ReadAsStringAsync();
                return (List<MetricResponse>)JsonConvert.DeserializeObject(response, typeof(List<MetricResponse>));
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
