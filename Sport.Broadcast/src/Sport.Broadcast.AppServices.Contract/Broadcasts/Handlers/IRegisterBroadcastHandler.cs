using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers
{
    /// <summary>
    /// Обработчик регистрации новой трансляции.
    /// </summary>
    public interface IRegisterBroadcastHandler
    {
        /// <summary>
        /// Зарегистрировать новую трансляцию.
        /// </summary>
        /// <param name="request">Данные для регистрации трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Зарегистрированная трансляция.</returns>
        Task<SportBroadcast> Handle(BroadcastInternalRequest request, CancellationToken token);
    }
}