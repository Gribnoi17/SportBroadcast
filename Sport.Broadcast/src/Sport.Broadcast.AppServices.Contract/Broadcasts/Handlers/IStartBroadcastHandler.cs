using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers
{
    /// <summary>
    /// Обработчик для запуска трансляции.
    /// </summary>
    public interface IStartBroadcastHandler
    {
        /// <summary>
        /// Запустить трансляцию с определенным айди.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Трансляция, результат запуска трансляции.</returns>
        Task<(SportBroadcast, bool)> Handle(long broadcastId, CancellationToken token);
    }
}