using Serilog;

namespace Sport.Broadcast.Host.Infrastructure.Extensions
{
    /// <summary>
    /// Расширения для настройки логирования.
    /// </summary>
    public static class LoggerExtensions
    {
        private const string _loggerSection = "Logging";
        
        /// <summary>
        /// Регистрирует Logger с настройками и поддержкой Serilog.
        /// </summary>
        /// <param name="services">Коллекция сервисов для регистрации Logger.</param>
        /// <param name="configuration">Конфигурация, содержащая наименование службы.</param>
        /// <returns>Обновленную коллекцию сервисов.</returns>
        public static IServiceCollection AddLoggerService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder => builder
                .AddConfiguration(configuration.GetSection(_loggerSection))
                .AddConsole());
            
            services.AddSerilog(cfg => cfg.ReadFrom.Configuration(configuration.GetSection(_loggerSection)));

            return services;
        }
    }
}