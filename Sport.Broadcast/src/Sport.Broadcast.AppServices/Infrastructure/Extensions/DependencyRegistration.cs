using Microsoft.Extensions.DependencyInjection;
using Sport.Broadcast.AppServices.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Broadcasts.Validators;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Validators;

namespace Sport.Broadcast.AppServices.Infrastructure.Extensions
{
    /// <summary>
    /// Класс, предоставляющий методы для регистрации зависимостей в контейнере DI.
    /// </summary>
    public static class DependencyRegistration
    {
        /// <summary>
        /// Регистрирует обработчики для операций с клиентами и кредитными договорами.
        /// </summary>
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<ISendMessageHandler, SendMessageHandler>();
            
            services.AddScoped<IRegisterBroadcastHandler, RegisterBroadcastHandler>();
            
            services.AddScoped<ISearchBroadcastHandler, SearchBroadcastHandler>();
            
            services.AddScoped<IStartBroadcastHandler, StartBroadcastHandler>();
            
            services.AddScoped<IStopBroadcastHandler, StopBroadcastHandler>();
            
            services.AddScoped<IJoinBroadcastHandler, JoinBroadcastHandler>();
            
            services.AddScoped<IBuildMessageHandler, BuildMessageHandler>();
            
            return services;
        }
        
        /// <summary>
        /// Регистрирует валидаторы для проверки данных.
        /// </summary>
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IBroadcastValidator, BroadcastValidator>();
            
            return services;
        }
    }
}