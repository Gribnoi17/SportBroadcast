using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Hubs;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Requests;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.Host.Infrastructure.MapService;

namespace Sport.Broadcast.Host.Broadcasts.Hubs
{
    /// <inheritdoc cref="IBroadcastHub" />
    public class BroadcastHub : Hub<IBroadcastHub>
    {
        private const string _groupName = "broadcast_";
        private static readonly ConcurrentDictionary<long, string> _broadcastConnectionMap = new();

        private readonly ISendMessageHandler _sendMessageHandler;
        private readonly IStartBroadcastHandler _startBroadcastHandler;
        private readonly IStopBroadcastHandler _stopBroadcastHandler;
        private readonly IJoinBroadcastHandler _joinBroadcastHandler;
        private readonly IBuildMessageHandler _buildMessageHandler;
        private readonly ILogger<BroadcastHub> _logger;

        /// <summary>
        /// Инициализирует объект класса BroadcastHub.
        /// </summary>
        /// <param name="sendMessageHandler">Обработчик отправки сообщений.</param>
        /// <param name="startBroadcastHandler">Обработчик запуска трансляции.</param>
        /// <param name="stopBroadcastHandler">Обработчик остановки трансляции.</param>
        /// <param name="joinBroadcastHandler">Обработчик подключения к трансляции.</param>
        /// <param name="buildMessageHandler">Обработчик для сборки сообщения.</param>
        /// <param name="logger">Logger сообщений.</param>
        public BroadcastHub(ISendMessageHandler sendMessageHandler,
            IStartBroadcastHandler startBroadcastHandler,
            IStopBroadcastHandler stopBroadcastHandler,
            IJoinBroadcastHandler joinBroadcastHandler,
            IBuildMessageHandler buildMessageHandler,
            ILogger<BroadcastHub> logger)
        {
            _sendMessageHandler = sendMessageHandler;
            _startBroadcastHandler = startBroadcastHandler;
            _stopBroadcastHandler = stopBroadcastHandler;
            _joinBroadcastHandler = joinBroadcastHandler;
            _logger = logger;
            _buildMessageHandler = buildMessageHandler;
        }

        /// <summary>
        /// Запустить трансляцию по ее Id.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        public async Task StartBroadcast(long broadcastId)
        {
            _logger.LogInformation("Начался процесс запуска трансляции с Id: {broadcastId}.", broadcastId);
            
            var result = await _startBroadcastHandler.Handle(broadcastId, CancellationToken.None);

            if (result.Item2)
            {
                var broadcastResponse = MappingService.MapToBroadcastResponse(result.Item1);

                await Clients.Caller.StartBroadcast(broadcastResponse);

                _logger.LogInformation("Трансляции с Id: {broadcastId} успешно запущена.", broadcastId);
                
                return;
            }
            
            await Clients.Caller.ReceiveError($"Запустить трансляцию раньше времени нельзя! Трансляция зарегистрирована на {result.Item1.StartTime}.");
        }

        /// <summary>
        /// Добавление болельщика к трансляции по ее Id.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        public async Task JoinBroadcast(long broadcastId)
        {
            _logger.LogInformation(
                "Начался процесс подключения болельщика с Id подключения: {ConnectionId} к трансляции с Id: {broadcastId}.",
                Context.ConnectionId, broadcastId);

            if (await _joinBroadcastHandler.Handle(broadcastId, CancellationToken.None))
            {
                _logger.LogTrace("Добавляем болельщика с Id: {ConnectionId} в группу трансляции c Id: {broadcastId}.", Context.ConnectionId, broadcastId);
                
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{_groupName}{broadcastId}");

                _broadcastConnectionMap.TryAdd(broadcastId, Context.ConnectionId);

                await Clients.Caller.JoinBroadcast("Вы успешно подключились к трансляции.", true);
                
                _logger.LogInformation(
                    "Болельщик с Id подключения: {ConnectionId} успешно подключился к трансляции с Id: {broadcastId}.",
                    Context.ConnectionId, broadcastId);

                return;
            }

            await Clients.Caller.JoinBroadcast("Подключение к трансляции провалилось. Трансляция не запущена.", false);
            
            _logger.LogInformation(
                "Подключения болельщика с Id подключения: {ConnectionId} к трансляции с Id: {broadcastId} провалилось.",
                Context.ConnectionId, broadcastId);
        }

        /// <summary>
        /// Отключение от трансляции с определенным Id.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        public async Task LeaveBroadcast(long broadcastId)
        {
            _logger.LogInformation(
                "Начался процесс отключения болельщика с Id подключения: {ConnectionId} от трансляции с Id: {broadcastId}.",
                Context.ConnectionId, broadcastId);
            
            await Clients.Caller.LeaveBroadcast("Вы покинули трансляцию.");
            
            _logger.LogTrace("Удаляем болельщика с Id: {ConnectionId} из группы трансляции c Id: {broadcastId}.", Context.ConnectionId, broadcastId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{_groupName}{broadcastId}");

            var entryToRemove = _broadcastConnectionMap.FirstOrDefault(entry => entry.Value == Context.ConnectionId);

            _broadcastConnectionMap.TryRemove(entryToRemove.Key, out _);
            
            _logger.LogInformation(
                "Болельщика с Id подключения: {ConnectionId} успешно отключился от трансляции с Id: {broadcastId}.",
                Context.ConnectionId, broadcastId);
        }

        /// <summary>
        /// Отправить сообщение всем, кто подключен к трансляции.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        /// <param name="message">Сообщение.</param>
        public async Task SendMessage(long broadcastId, MessageRequest message)
        {
            _logger.LogInformation(
                "Начался процесс обработки сообщения от комментатора с Id подключения: {ConnectionId} для трансляции с Id: {broadcastId}.",
                Context.ConnectionId, broadcastId);
            
            var messageInternalRequest = MappingService.MapToBroadcastMessageInternalRequest(message);

            var result = await _buildMessageHandler.Handle(messageInternalRequest);

            await _sendMessageHandler.Handle(broadcastId, messageInternalRequest);

            await Clients.Group($"{_groupName}{broadcastId}").ReceiveMessage(result);
            
            _logger.LogInformation(
                "Сообщение от комментатора с Id подключения: {ConnectionId} для трансляции с Id: {broadcastId} успешно доставлено болельщикам.",
                Context.ConnectionId, broadcastId);
        }

        /// <summary>
        /// Остановка трансляции с определенным Id.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        public async Task StopBroadcast(long broadcastId)
        {
            _logger.LogInformation("Начался процесс остановки трансляции с Id: {broadcastId}.", broadcastId);
            
            await _stopBroadcastHandler.Handle(broadcastId, CancellationToken.None);

            await Clients.Group($"{_groupName}{broadcastId}").StopBroadcast(broadcastId);

            _logger.LogTrace($"Исключаем из группы с Id трансляции: {broadcastId} всех подключенных болельщиков.");
            var keysToRemove = _broadcastConnectionMap.Where(pair => pair.Key == broadcastId).Select(pair => pair.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                await Groups.RemoveFromGroupAsync(_broadcastConnectionMap[key], $"{_groupName}{key}");
                _broadcastConnectionMap.TryRemove(key, out _);
            }

            await Clients.Caller.StopBroadcast(broadcastId);
            
            _logger.LogInformation("Остановки трансляции с Id: {broadcastId} прошла успешно.", broadcastId);
        }
    }
}