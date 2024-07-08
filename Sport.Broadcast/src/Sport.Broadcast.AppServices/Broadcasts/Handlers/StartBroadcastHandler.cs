using Microsoft.Extensions.Logging;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.Broadcasts.Handlers
{   
    /// <inheritdoc cref="IStartBroadcastHandler"/>
    internal class StartBroadcastHandler : IStartBroadcastHandler
    {
        private readonly IBroadcastRepository _repository;
        private readonly ILogger<StartBroadcastHandler> _logger;

        /// <summary>
        /// Инициализирует новый объект класса StartBroadcastHandler.
        /// </summary>
        /// <param name="repository">Репозиторий спортивных трансляций.</param>
        /// <param name="logger">Logger сообщений.</param>
        public StartBroadcastHandler(IBroadcastRepository repository, ILogger<StartBroadcastHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<(SportBroadcast, bool)> Handle(long broadcastId, CancellationToken token)
        {
            _logger.LogTrace("Получение трансляции с Id: {broadcastId} из базы данных.", broadcastId);
            
            var broadcast = await _repository.GetSportBroadcast(broadcastId, token);

            var currentTime = DateTime.Now;

            if (currentTime < broadcast.StartTime)
            {
                return (broadcast, false);
            }
            
            UpdateBroadcast(broadcast);
            
            _logger.LogInformation("Обновили данные трансляции с Id: {broadcastId} для ее запуска.", broadcastId);

            await _repository.UpdateSportBroadcast(broadcast, token);
            
            _logger.LogTrace("Данные трансляции с Id: {broadcastId} обновлены в базе данных.", broadcastId);

            return (broadcast, true);
        }
        
        private void UpdateBroadcast(SportBroadcast sportBroadcast)
        {
            sportBroadcast.Status = BroadcastStatus.Started;
            sportBroadcast.ScoreOfHomeTeam = 0;
            sportBroadcast.ScoreOfGuestTeam = 0;
            sportBroadcast.ExtraTime = 0;
            sportBroadcast.TotalExtraTime = 0;
            sportBroadcast.CurrentHalf = 1;
        }
    }
}