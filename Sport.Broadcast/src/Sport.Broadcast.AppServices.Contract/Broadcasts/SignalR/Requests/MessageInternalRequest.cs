using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Enums;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests
{
    /// <summary>
    /// Модель сообщения от комментатора для слоя бизнес логики.
    /// </summary>
    public class MessageInternalRequest
    {
        /// <summary>
        /// Минута матча.
        /// </summary>
        public string? Minute { get; set; }
        
        /// <summary>
        /// Событие в матче.
        /// </summary>
        public EventInGame? Event { get; set; }
        
        /// <summary>
        /// Счет в матче.
        /// </summary>
        public string? Score { get; set; }
        
        /// <summary>
        /// Номер тайма после перерыва.
        /// </summary>
        public int? HalfNumberAfterBreak { get; set; }
        
        /// <summary>
        /// Дополнительное время в матче.
        /// </summary>
        public int? ExtraTime { get; set; }
        
        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string? PlayerName { get; set; }
        
        /// <summary>
        /// Текст комментатора.
        /// </summary>
        public string Text { get; set; }
    }
}