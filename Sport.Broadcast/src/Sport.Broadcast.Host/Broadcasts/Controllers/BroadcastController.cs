using Microsoft.AspNetCore.Mvc;
using Sport.Broadcast.Api.Contracts.Broadcasts.Controllers;
using Sport.Broadcast.Api.Contracts.Broadcasts.Requests;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.Host.Infrastructure.Binders;
using Sport.Broadcast.Host.Infrastructure.Filters;
using Sport.Broadcast.Host.Infrastructure.MapService;

namespace Sport.Broadcast.Host.Broadcasts.Controllers
{
    /// <inheritdoc cref="IBroadcastController" />
    
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    [Route("broadcasts")]
    public class BroadcastController : Controller, IBroadcastController
    {
        private readonly IRegisterBroadcastHandler _registerBroadcastHandler;
        private readonly ISearchBroadcastHandler _searchBroadcastHandler;

        /// <summary>
        /// Инициализирует объект класса SportBroadcastController.
        /// </summary>
        /// <param name="registerBroadcastHandler">Обработчик для регистрации трансляции.</param>
        /// <param name="searchBroadcastHandler">Обработчик для поиска трансляций.</param>
        public BroadcastController(IRegisterBroadcastHandler registerBroadcastHandler, ISearchBroadcastHandler searchBroadcastHandler)
        {
            _registerBroadcastHandler = registerBroadcastHandler;
            _searchBroadcastHandler = searchBroadcastHandler;
        }

        [HttpPost]
        [Route("register")]
        public async Task<BroadcastResponse> RegisterBroadcast(BroadcastRegistrationRequest request, CancellationToken token)
        {
            var registrationInternalRequest = MappingService.MapToBroadcastRegistrationInternalRequest(request);

            var broadcast = await _registerBroadcastHandler.Handle(registrationInternalRequest, token);

            var response = MappingService.MapToBroadcastResponse(broadcast);

            return response;
        }

        [HttpGet]
        public async Task<BroadcastResponse[]> GetBroadcasts([ModelBinder(BinderType = typeof(DateOnlyModelBinder))] DateOnly date, CancellationToken token)
        {
            var broadcasts = await _searchBroadcastHandler.Handle(date, token);

            var response = MappingService.MapToBroadcastResponses(broadcasts);

            return response.ToArray();
        }
    }
}