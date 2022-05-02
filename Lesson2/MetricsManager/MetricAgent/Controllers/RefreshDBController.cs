using MetricAgent.DAL.Models;
using MetricAgent.Requests;
using MetricAgent.Responses;
using MetricAgent.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using Dapper;
using MetricAgent.MySQLSettings;

namespace MetricAgent.Controllers
{
    /// <summary>
    /// Контроллер для обработки Cpu метрик
    /// </summary>
    [Route("api/metrics")]
    [ApiController]
    public class RefreshDBController : ControllerBase
    {
        private readonly ILogger<CPUMetricsController> _logger;

        public RefreshDBController(ILogger<CPUMetricsController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Чистка БД по желанию
        /// </summary>
        /// <returns>Удаляет все из БД</returns>
        [HttpGet("refresh")]
        public IActionResult Refresh()
        {
            IMySqlSettings mySql = new MySqlSettings();
            var connection = new SQLiteConnection(mySql.ConnectionString);
            
            connection.ExecuteAsync($"DROP TABLE IF EXISTS {mySql[Tables.CPUMetric]}");
            connection.ExecuteAsync(
                $"CREATE TABLE {mySql[Tables.CPUMetric]} " +
                $"({mySql[Columns.Id]} INTEGER PRIMARY KEY, {mySql[Columns.Value]} INT, {mySql[Columns.Time]} INTEGER)");

            return Ok("БД - очищена!");
        }
    }
}
