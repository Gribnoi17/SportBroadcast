using Sport.Broadcast.Host.Infrastructure.Middleware;

namespace Sport.Broadcast.Host.Infrastructure.Extensions
{
    /// <summary>
    /// Расширения для конфигурации Middleware.
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Регистрирует промежуточное Middleware для добавления HTTP-заголовка "ServiceName" с указанным значением из конфигурации.
        /// </summary>
        /// <param name="services">Коллекция сервисов для регистрации Middleware.</param>
        /// <param name="configuration">Конфигурация, содержащая наименование службы.</param>
        /// <returns>Обновленную коллекцию сервисов.</returns>
        public static IServiceCollection AddMiddlewareService(this IServiceCollection services, IConfiguration configuration)
        {
            string serviceName = configuration.GetValue<string>("ServiceName") ??
                                 throw new ArgumentNullException("configuration.GetValue<string>(\"ServiceName\")");

            services.AddTransient(provider => new HeaderMiddleware(serviceName));

            return services;
        }
    }
}