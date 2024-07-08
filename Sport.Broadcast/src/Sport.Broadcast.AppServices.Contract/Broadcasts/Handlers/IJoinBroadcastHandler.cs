namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers
{
    /// <summary>
    /// Обработчик для подключения трансляции.
    /// </summary>
    public interface IJoinBroadcastHandler
    {
        /// <summary>
        /// Подключиться к трансляции с определенным айди.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Результат подключения.</returns>
        Task<bool> Handle(long broadcastId, CancellationToken token);
    }
}