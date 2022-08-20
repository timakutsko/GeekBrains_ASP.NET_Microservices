using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricAgent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Запуск");
                CreateHostBuilder(args).Build().Run();
            }
            // Отлов всех исключений в рамках работыприложения
            catch (Exception exception)
            {
                //NLog: устанавливаем отлов исключений
                logger.Error(exception, "Остановлено из-за ошибки");
            }
            // Остановка логгера
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); // Созданиепровайдеров логирования
                    logging.SetMinimumLevel(LogLevel.Trace); //Устанавливаем минимальный уровень логирования
                }).UseNLog(); // Добавляем библиотекуnlog
    }
}
