using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.DataAccess.Broadcasts.Repository;
using Sport.Broadcast.DataAccess.Infrastructure.Data;

namespace Sport.Broadcast.DataAccess.Infrastructure.Extensions
{
    /// <summary>
    /// Класс, предоставляющий методы для регистрации зависимостей в контейнере DI.
    /// </summary>
    public static class DependencyRegistration
    {
        private const string _databaseConnectionStringSection = "DatabaseConnectionString";
        private const string _defaultConnection = "DefaultConnection";
        
        /// <summary>
        /// Регистрирует репозитории для работы с базой данных.
        /// </summary>
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IBroadcastRepository, BroadcastRepository>();

            return services;
        }
        
        /// <summary>
        /// Регистрирует контекст базы данных.
        /// </summary>
        public static IServiceCollection AddSportBroadcastDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection(_databaseConnectionStringSection)[_defaultConnection];
            
            services.AddDbContext<BroadcastDbContext>(
                options => options.UseNpgsql(connectionString));

            return services;
        }
    }
}