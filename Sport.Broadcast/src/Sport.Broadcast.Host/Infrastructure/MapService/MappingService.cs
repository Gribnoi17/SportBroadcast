using Sport.Broadcast.Api.Contracts.Broadcasts.Enums;
using Sport.Broadcast.Api.Contracts.Broadcasts.Requests;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Requests;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;

namespace Sport.Broadcast.Host.Infrastructure.MapService
{
    /// <summary>
    /// Класс для маппинга данных.
    /// </summary>
    internal static class MappingService
    {
        /// <summary>
        /// Маппинг данных из запроса регистрации во внутреннюю модель.
        /// </summary>
        /// <param name="request">Запрос на регистрацию трансляции.</param>
        /// <returns>Внутренняя модель запроса регистрации трансляции.</returns>
        public static BroadcastInternalRequest MapToBroadcastRegistrationInternalRequest(BroadcastRegistrationRequest request)
        {
            return new BroadcastInternalRequest
            {
                GuestTeam = request.GuestTeam,
                HomeTeam = request.HomeTeam,
                StartTime = request.StartTime
            };
        }
        
        /// <summary>
        /// Маппинг данных из внутренней модели трансляции в ответ пользователю.
        /// </summary>
        /// <param name="sportBroadcast">Внутренняя модель трансляции.</param>
        /// <returns>Ответ пользователю на запрос о трансляции.</returns>
        public static BroadcastResponse MapToBroadcastResponse(SportBroadcast sportBroadcast)
        {
            var response = new BroadcastResponse
            {
                Id = sportBroadcast.Id,
                GuestTeam = sportBroadcast.GuestTeam,
                HomeTeam = sportBroadcast.HomeTeam,
                StartTime = sportBroadcast.StartTime,
                ScoreOfHomeTeam = sportBroadcast.ScoreOfHomeTeam,
                ScoreOfGuestTeam = sportBroadcast.ScoreOfGuestTeam,
                CurrentHalf = sportBroadcast.CurrentHalf,
                ExtraTime = sportBroadcast.ExtraTime,
                TotalExtraTime = sportBroadcast.TotalExtraTime,
                Status = MapToBroadcastStatus(sportBroadcast.Status)
            };

            return response;
        }
        
        /// <summary>
        /// Маппинг данных из внутренней модели трансляции в ответ пользователю.
        /// </summary>
        /// <param name="message">Сообщение от комментатора.</param>
        /// <returns>Сообщение от комментатора для слоя бизнес логики.</returns>
        public static MessageInternalRequest MapToBroadcastMessageInternalRequest(MessageRequest message)
        {
            var response = new MessageInternalRequest
            {
                Minute = message.Minute,
                Event = MapToInternalEventInGame(message.Event),
                Score = message.Score,
                HalfNumberAfterBreak = message.HalfNumberAfterBreak,
                ExtraTime = message.ExtraTime,
                PlayerName = message.PlayerName,
                Text = message.Text
            };

            return response;
        }
        
        /// <summary>
        /// Маппинг списка внутренних моделей трансляции в ответы пользователю.
        /// </summary>
        /// <param name="broadcasts">Список внутренних моделей трансляции.</param>
        /// <returns>Массив ответов пользователю на запросы о трансляциях.</returns>
        public static BroadcastResponse[] MapToBroadcastResponses(List<SportBroadcast> broadcasts)
        {
            var broadcastResponses = new List<BroadcastResponse>();

            foreach (var broadcast in broadcasts)
            {
                var response = MapToBroadcastResponse(broadcast);
                broadcastResponses.Add(response);
            }

            return broadcastResponses.ToArray();
        }
        
        /// <summary>
        /// Маппинг события, которое произошло в игре во время трансляции, между моделью для ответа клиента и внутренней моделью.
        /// </summary>
        /// <param name="eventInGame">Событие во время трансляции.</param>
        /// <returns>Событие в игре во время трансляции для слоя бизнес логики.</returns>
        public static EventInGame? MapToInternalEventInGame(Api.Contracts.Broadcasts.SignalR.Enums.EventInGame? eventInGame)
        {
            return eventInGame switch
            {
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.Goal => EventInGame.Goal,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.Var => EventInGame.Var,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.VarGoalCancellation => EventInGame.VarGoalCancellation,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.Substitution => EventInGame.Substitution,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.YellowCard => EventInGame.YellowCard,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.SecondYellowCard => EventInGame.SecondYellowCard,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.RedCard => EventInGame.RedCard,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.Break => EventInGame.Break,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.ExtraTime => EventInGame.ExtraTime,
                Api.Contracts.Broadcasts.SignalR.Enums.EventInGame.BeginningHalf => EventInGame.BeginningHalf,
                null => null,
                _ => throw new InvalidCastException($"Произошла ошибка при маппинге события: {eventInGame}")
            };
        }
        
        /// <summary>
        /// Маппинг статуса трансляции между внутренней моделью и моделью для ответа пользователю.
        /// </summary>
        /// <param name="status">Статус внутренней модели трансляции.</param>
        /// <returns>Статус трансляции для ответа пользователю.</returns>
        public static BroadcastStatus MapToBroadcastStatus(AppServices.Contract.Broadcasts.Enums.BroadcastStatus status)
        {
            return status switch
            {
                AppServices.Contract.Broadcasts.Enums.BroadcastStatus.NotStarted => BroadcastStatus.NotStarted,
                AppServices.Contract.Broadcasts.Enums.BroadcastStatus.Started => BroadcastStatus.Started,
                AppServices.Contract.Broadcasts.Enums.BroadcastStatus.Ended => BroadcastStatus.Ended,
                _ => throw new InvalidCastException($"Произошла ошибка при маппинге статуса: {status}")
            };
        }
    }
}