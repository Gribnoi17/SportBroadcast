using Microsoft.Extensions.Logging;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.Broadcasts.Handlers
{
    /// <inheritdoc cref="ISearchBroadcastHandler" />
    internal class SearchBroadcastHandler : ISearchBroadcastHandler
    {
        private readonly IBroadcastRepository _repository;
        private readonly ILogger<SearchBroadcastHandler> _logger;

        /// <summary>
        /// Инициализирует новый объект класса SearchBroadcastHandler.
        /// </summary>
        /// <param name="repository">Репозиторий спортивных трансляций.</param>
        /// <param name="logger">Logger сообщений.</param>
        public SearchBroadcastHandler(IBroadcastRepository repository, ILogger<SearchBroadcastHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<SportBroadcast>> Handle(DateOnly date, CancellationToken token)
        {
            _logger.LogInformation("Получение списка трансляций на дату: {date}", date);
            
             var broadcasts = await _repository.GetSportBroadcasts(date, token);

             return broadcasts;
        }
    }
}