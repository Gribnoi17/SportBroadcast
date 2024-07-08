namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers
{
    /// <summary>
    /// Обработчик для завершения трансляции.
    /// </summary>
    public interface IStopBroadcastHandler
    {
        /// <summary>
        /// Завершить трансляцию с определенным айди.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns></returns>
        Task Handle(long broadcastId, CancellationToken token);
    }
}