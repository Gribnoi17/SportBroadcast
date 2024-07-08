using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Microsoft.Extensions.Logging;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;

namespace Sport.Broadcast.AppServices.Broadcasts.Handlers
{
    /// <inheritdoc cref="IJoinBroadcastHandler"/>
    internal class JoinBroadcastHandler : IJoinBroadcastHandler
    {
        private readonly IBroadcastRepository _repository;
        private readonly ILogger<JoinBroadcastHandler> _logger;

        /// <summary>
        /// Инициализирует новый объект класса JoinBroadcastHandler.
        /// </summary>
        /// <param name="repository">Репозиторий спортивных трансляций.</param>
        /// <param name="logger">Logger сообщений.</param>
        public JoinBroadcastHandler(IBroadcastRepository repository, ILogger<JoinBroadcastHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(long broadcastId, CancellationToken token)
        {
            _logger.LogTrace("Получение трансляции из базы данных с Id: {broadcastId}", broadcastId);
            
            var broadcast = await _repository.GetSportBroadcast(broadcastId, token);

            return broadcast.Status == BroadcastStatus.Started;
        }
    }
}