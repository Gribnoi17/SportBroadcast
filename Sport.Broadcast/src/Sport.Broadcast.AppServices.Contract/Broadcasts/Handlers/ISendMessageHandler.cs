using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers
{
    /// <summary>
    /// Обработчик отправки сообщения.
    /// </summary>
    public interface ISendMessageHandler
    {
        /// <summary>
        /// Отправить сообщение о произошедшем в матче событии с обновлением базы данных.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        /// <param name="message">Текст сообщения.</param>
        /// <returns></returns>
        Task Handle(long broadcastId, MessageInternalRequest message);
    }
}