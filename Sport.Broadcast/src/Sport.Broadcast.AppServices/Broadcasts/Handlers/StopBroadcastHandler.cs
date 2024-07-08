using Microsoft.Extensions.Logging;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.Broadcasts.Handlers
{
    /// <inheritdoc cref="IStopBroadcastHandler"/>
    internal class StopBroadcastHandler : IStopBroadcastHandler
    {
        private readonly IBroadcastRepository _repository;
        private readonly ILogger<StopBroadcastHandler> _logger;

        /// <summary>
        /// Инициализирует новый объект класса StopBroadcastHandler.
        /// </summary>
        /// <param name="repository">Репозиторий спортивных трансляций.</param>
        /// <param name="logger">Logger сообщений.</param>
        public StopBroadcastHandler(IBroadcastRepository repository, ILogger<StopBroadcastHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(long broadcastId, CancellationToken token)
        {
            _logger.LogTrace($"Получаем трансляцию с Id: {broadcastId} из базы данных.");
            
            var broadcast = await _repository.GetSportBroadcast(broadcastId, token);
            
            UpdateBroadcast(broadcast);
            
            _logger.LogInformation($"Обновили данные трансляции: {broadcastId} для ее остановки.");

            await _repository.UpdateSportBroadcast(broadcast, token);
        }

        private void UpdateBroadcast(SportBroadcast sportBroadcast)
        {
            sportBroadcast.Status = BroadcastStatus.Ended;
            sportBroadcast.CurrentHalf = 0;
            sportBroadcast.ExtraTime = 0;
        }
    }
}