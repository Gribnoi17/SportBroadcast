using System.ComponentModel;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;

namespace Sport.Broadcast.AppServices.Broadcasts.Handlers
{
    /// <inheritdoc cref="IBuildMessageHandler"/>
    internal class BuildMessageHandler : IBuildMessageHandler
    {
        private readonly ILogger<BuildMessageHandler> _logger;

        /// <summary>
        /// Инициализирует новый объект класса BuildMessageHandler.
        /// </summary>
        /// <param name="logger">Logger сообщений.</param>
        public BuildMessageHandler(ILogger<BuildMessageHandler> logger)
        {
            _logger = logger;
        }
        
        public Task<string> Handle(MessageInternalRequest message)
        {
            var result = BuildStringResult(message);
            
            _logger.LogInformation("Сообщение болельщкам от комментатора собрано.");

            return Task.FromResult(result);
        }
        
        /// <summary>
        /// Собирает сообщение от комментатора в единый результат.
        /// </summary>
        /// <param name="message">Информация о сообщении.</param>
        /// <returns>Сообщение комментатора.</returns>
        internal static string BuildStringResult(MessageInternalRequest message)
        {
            var result = new StringBuilder()
                .Append($"{message.Minute};");

            if (message.Event.HasValue)
            {
                result.Append($"{GetEventDescription(message.Event)};");
            }
            
            if (!string.IsNullOrWhiteSpace(message.Score) && message.Event != EventInGame.VarGoalCancellation)
            {
                result.Append($"{message.Score};");
            }

            if (!string.IsNullOrWhiteSpace(message.PlayerName))
            {
                result.Append($"{message.PlayerName};");
            }

            result.Append($"{message.Text}");

            return result.ToString();
        }
        
        /// <summary>
        /// Получение описания события в матче на русском.
        /// </summary>
        /// <param name="eventInGame">Событие в матче.</param>
        /// <returns>описания события в матче на русском.</returns>
        internal static string GetEventDescription(EventInGame? eventInGame)
        {
            var descriptionAttribute = eventInGame?.GetType()
                .GetField(eventInGame.ToString() ?? string.Empty)?
                .GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;

            return descriptionAttribute?.Description ?? eventInGame?.ToString() ?? string.Empty;
        }
    }
}