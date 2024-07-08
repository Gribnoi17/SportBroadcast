using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;

namespace Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Hubs
{
    /// <summary>
    /// Предназначен для работы с SignlarR в реальном времени.
    /// </summary>
    public interface IBroadcastHub
    {
        /// <summary>
        /// Запуск трансляции с определенным Id.
        /// </summary>
        /// <param name="broadcastResponse">Информация о запущенная трансляция.</param>
        /// <returns></returns>
        Task StartBroadcast(BroadcastResponse broadcastResponse);

        /// <summary>
        /// Подключение к трансляции.
        /// </summary>
        /// <param name="message">Сообщение об результате подключения.</param>
        /// <param name="isSuccess">Флаг о результате подключения</param>
        /// <returns></returns>
        Task JoinBroadcast(string message, bool isSuccess);

        /// <summary>
        /// Отключиться от трансляции.
        /// </summary>
        /// <param name="message">Сообщение об отключении.</param>
        /// <returns></returns>
        Task LeaveBroadcast(string message);

        /// <summary>
        /// Получение сообщения болельщиками о произошедшем в матче событии.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <returns></returns>
        Task ReceiveMessage(string message);

        /// <summary>
        /// Завершение трансляции с определенным Id.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        /// <returns></returns>
        Task StopBroadcast(long broadcastId);
        
        /// <summary>
        /// Получение клиентами сообщения об ошибке.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        /// <returns></returns>
        Task ReceiveError(string message);
    }
}