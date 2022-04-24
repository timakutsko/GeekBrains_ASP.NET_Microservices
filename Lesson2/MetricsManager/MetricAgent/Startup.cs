using MetricAgent.Requests;
using MetricAgent.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricAgent.DAL.Models;

namespace MetricAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter()));
            ConfigureSqlLiteConnection(services);
            services.AddSingleton<ICPUMetricsRepository, CPUMetricsRepository>();
            services.AddSingleton<IHDDMetricsRepository, HDDMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRAMMetricsRepository, RAMMetricsRepository>();
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services) 
        { 
            const string connectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";
            var connection = new SQLiteConnection(connectionString); 
            connection.Open(); 
            PrepareSchema(connection); 
        }
        private void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            { 
                // Задаём новый текст команды для выполнения 
                // Удаляем таблицу с метриками, если она есть в базе данных
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics"; 
                command.CommandText = "DROP TABLE IF EXISTS hddmetrics"; 
                command.CommandText = "DROP TABLE IF EXISTS netmetrics"; 
                command.CommandText = "DROP TABLE IF EXISTS networkmetrics"; 
                command.CommandText = "DROP TABLE IF EXISTS rammetrics"; 
                // Отправляем запрос в базу данных
                command.ExecuteNonQuery(); 
                command.CommandText = @"CREATE TABLE cpumetrics (id INTEGER PRIMARY KEY, value INT, time INT)"; 
                command.CommandText = @"CREATE TABLE hddmetrics (id INTEGER PRIMARY KEY, value INT, time INT)"; 
                command.CommandText = @"CREATE TABLE netmetrics (id INTEGER PRIMARY KEY, value INT, time INT)"; 
                command.CommandText = @"CREATE TABLE networkmetrics (id INTEGER PRIMARY KEY, value INT, time INT)"; 
                command.CommandText = @"CREATE TABLE rammetrics (id INTEGER PRIMARY KEY, value INT, time INT)"; 
                command.ExecuteNonQuery(); 
            } 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
