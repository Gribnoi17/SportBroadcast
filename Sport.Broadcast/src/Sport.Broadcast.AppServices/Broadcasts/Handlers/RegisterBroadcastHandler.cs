using Microsoft.Extensions.Logging;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Validators;

namespace Sport.Broadcast.AppServices.Broadcasts.Handlers
{
    /// <inheritdoc cref="IRegisterBroadcastHandler" />
    internal class RegisterBroadcastHandler : IRegisterBroadcastHandler
    {
        private readonly IBroadcastValidator _validator;
        private readonly IBroadcastRepository _repository;
        private readonly ILogger<RegisterBroadcastHandler> _logger;

        /// <summary>
        /// Инициализирует новый объект класса RegisterBroadcastHandler.
        /// </summary>
        /// <param name="repository">Репозиторий спортивных трансляций.</param>
        /// <param name="validator">Валидатор данных для регистрации спортивной трансляции.</param>
        /// <param name="logger">Logger сообщений.</param>
        public RegisterBroadcastHandler(IBroadcastRepository repository, IBroadcastValidator validator, ILogger<RegisterBroadcastHandler> logger)
        {
            _repository = repository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<SportBroadcast> Handle(BroadcastInternalRequest request, CancellationToken token)
        {
            _logger.LogInformation("Начался процесс регистрациии трансляции.");
            
            _validator.Validate(request);
            
            var broadcast = new SportBroadcast
            {
                GuestTeam = request.GuestTeam,
                HomeTeam = request.HomeTeam,
                StartTime = request.StartTime
            };
            var id = await _repository.RegisterSportBroadcast(broadcast, token);
            broadcast.Id = id;
            
            _logger.LogTrace("Трансляция с Id: {id} добавлена в базу данных.", id);

            _logger.LogInformation("Трансляция успешно зарегистрирована. Ее Id: {id}.", id);
            
            return broadcast;
        }
    }
}