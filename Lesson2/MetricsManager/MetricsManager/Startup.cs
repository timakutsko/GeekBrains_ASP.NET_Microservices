using AutoMapper;
using FluentMigrator.Runner;
using MetricsManager.DAL.Repositories;
using MetricsManager.MySQLSettings;
using MetricsManager.ScheduledWorks.Jobs;
using MetricsManager.ScheduledWorks.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
{
    public class Startup
    {
        /// <summary>
        /// Интервал запуска каждые 5 сек
        /// </summary>
        private const string _сronExpression = "0/5 * * * * ?";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Подключаю контроллеры
            services.AddControllers();

            // Создание DI-контейнеров для метрик
            services.AddSingleton<ICPUMetricsRepository, CPUMetricsRepository>();
            services.AddSingleton<IHDDMetricsRepository, HDDMetricsRepository>();
            services.AddSingleton<INETMetricsRepository, NETMetricsRepository>();
            services.AddSingleton<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddSingleton<IRAMMetricsRepository, RAMMetricsRepository>();

            // Создание DI-контейнера для SQL
            services.AddSingleton<IMySqlSettings, MySqlSettings>();
            
            // Создание DI-контейнеров для задач
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Добавляю задачи для планировщика в DI-контейнеры
            services.AddSingleton<CPUMetricJob>();
            services.AddSingleton<HDDMetricJob>();
            services.AddSingleton<NETMetricJob>();
            services.AddSingleton<NetworkMetricJob>();
            services.AddSingleton<RAMMetricJob>();
            // Запускать с определнным интервалом
            services.AddSingleton(new JobScheduleDTO(jobType: typeof(CPUMetricJob), cronExpression: _сronExpression));
            services.AddSingleton(new JobScheduleDTO(jobType: typeof(HDDMetricJob), cronExpression: _сronExpression));
            services.AddSingleton(new JobScheduleDTO(jobType: typeof(NETMetricJob), cronExpression: _сronExpression));
            services.AddSingleton(new JobScheduleDTO(jobType: typeof(NetworkMetricJob), cronExpression: _сronExpression));
            services.AddSingleton(new JobScheduleDTO(jobType: typeof(RAMMetricJob), cronExpression: _сronExpression));

            // Добавляю маппер
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            // Добавляю миграцию СУБД
            services.AddFluentMigratorCore().ConfigureRunner(
                rb => rb.
                // Добавляем поддержку SQLite
                AddSQLite().
                // Устанавливаем строку подключения
                WithGlobalConnectionString(new MySqlSettings().ConnectionString).
                // Подсказываем, где искать классы с миграциями
                ScanIn(typeof(Startup).Assembly).For.Migrations()).AddLogging(lb => lb.AddFluentMigratorConsole());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
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


            // Запускаем миграции
            migrationRunner.MigrateUp();
        }
    }
}
