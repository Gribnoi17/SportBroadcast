using System.ComponentModel;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;

namespace Sport.Broadcast.AppServices.Broadcasts.Handlers
{
    /// <inheritdoc cref="ISendMessageHandler"/>
    internal class SendMessageHandler : ISendMessageHandler
    {
        private static readonly Dictionary<EventInGame, Action<SportBroadcast, MessageInternalRequest>> _updateMethods =
            new()
            {
                { EventInGame.ExtraTime, UpdateExtraTime },
                { EventInGame.Break, UpdateBreak },
                { EventInGame.Goal, UpdateGoal },
                { EventInGame.VarGoalCancellation, UpdateGoal },
                { EventInGame.BeginningHalf, UpdateAfterBreak }
            };
        
        private readonly IBroadcastRepository _repository;
        private readonly ILogger<SendMessageHandler> _logger;

        /// <summary>
        /// Инициализирует новый объект класса SendMessageHandler.
        /// </summary>
        /// <param name="repository">Репозиторий спортивных трансляций.</param>
        /// <param name="logger">Logger сообщений.</param>
        public SendMessageHandler(IBroadcastRepository repository, ILogger<SendMessageHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(long broadcastId, MessageInternalRequest message)
        {
            if (ShouldUpdateBroadcast(message.Event))
            {
                var broadcast = await _repository.GetSportBroadcast(broadcastId, CancellationToken.None);
                UpdateBroadcast(broadcast, message);
                await _repository.UpdateSportBroadcast(broadcast, CancellationToken.None);
                
                _logger.LogInformation("Трансляция с Id: {broadcastId} успешно обновлена.", broadcastId);
            }
        }

        /// <summary>
        /// Проверяет, нужное ли событие для обновления базы данных произошло в матче.
        /// </summary>
        /// <param name="eventInGame">Событие в матче.</param>
        /// <returns>Результат проверки.</returns>
        internal static bool ShouldUpdateBroadcast(EventInGame? eventInGame)
        {
            return eventInGame is EventInGame.Break or EventInGame.ExtraTime or EventInGame.Goal or EventInGame.BeginningHalf or EventInGame.VarGoalCancellation;
        }
        
        /// <summary>
        /// Обновляет данные в базе данных.
        /// </summary>
        /// <param name="sportBroadcast">Трансляция, которую нужно обновить.</param>
        /// <param name="message">Данные для обновления, которые предоставил комментатор.</param>
        internal static void UpdateBroadcast(SportBroadcast sportBroadcast, MessageInternalRequest message)
        {
            if (message.Event != null && _updateMethods.TryGetValue(message.Event.Value, out var updateMethod))
            {
                updateMethod(sportBroadcast, message);
            }
        }

        /// <summary>
        /// Обновление базы данных при добавлении дополнительного времени в мачте.
        /// </summary>
        /// <param name="sportBroadcast">Трансляция, которую нужно обновить.</param>
        /// <param name="message">Данные для обновления, которые предоставил комментатор.</param>
        internal static void UpdateExtraTime(SportBroadcast sportBroadcast, MessageInternalRequest message)
        {
            sportBroadcast.TotalExtraTime ??= 0;
            
            sportBroadcast.ExtraTime = message.ExtraTime;
            sportBroadcast.TotalExtraTime += sportBroadcast.ExtraTime;
        }
        
        /// <summary>
        /// Обновление базы данных при перерыве в мачте.
        /// </summary>
        /// <param name="sportBroadcast">Трансляция, которую нужно обновить.</param>
        /// <param name="message">Данные для обновления, которые предоставил комментатор.</param>
        internal static void UpdateBreak(SportBroadcast sportBroadcast, MessageInternalRequest message)
        {
            sportBroadcast.CurrentHalf = 0;
            sportBroadcast.ExtraTime = 0;
        }
        
        /// <summary>
        /// Обновление базы данных после перерыва в мачте.
        /// </summary>
        /// <param name="sportBroadcast">Трансляция, которую нужно обновить.</param>
        /// <param name="message">Данные для обновления, которые предоставил комментатор.</param>
        internal static void UpdateAfterBreak(SportBroadcast sportBroadcast, MessageInternalRequest message)
        {
            sportBroadcast.CurrentHalf = message.HalfNumberAfterBreak;
        }
        
        /// <summary>
        /// Обновление базы данных при изменении счета в мачте.
        /// </summary>
        /// <param name="sportBroadcast">Трансляция, которую нужно обновить.</param>
        /// <param name="message">Данные для обновления, которые предоставил комментатор.</param>
        internal static void UpdateGoal(SportBroadcast sportBroadcast, MessageInternalRequest message)
        {
            if (string.IsNullOrWhiteSpace(message.Score))
            {
                throw new InvalidOperationException("Не указан новый счет матча.");
            }
            
            var score = message.Score.Split('-');
            
            sportBroadcast.ScoreOfHomeTeam = int.Parse(score[0]);
            sportBroadcast.ScoreOfGuestTeam = int.Parse(score[1]);
        }
    }
}