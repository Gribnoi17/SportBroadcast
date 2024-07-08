using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers
{
    /// <summary>
    /// Обработчик для собирания сообщения от комментатора.
    /// </summary>
    public interface IBuildMessageHandler
    {
        /// <summary>
        /// Собрать сообщение о произошедшем в матче событии.
        /// </summary>
        /// <param name="message">Данные сообщения.</param>
        /// <returns>Готовое сообщение.</returns>
        Task<string> Handle(MessageInternalRequest message);
    }
}