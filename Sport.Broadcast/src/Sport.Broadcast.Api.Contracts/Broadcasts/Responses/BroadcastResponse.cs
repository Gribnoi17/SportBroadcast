using Sport.Broadcast.Api.Contracts.Broadcasts.Enums;

namespace Sport.Broadcast.Api.Contracts.Broadcasts.Responses
{
    /// <summary>
    /// Модель, которая содержит данные о трансляции для ответа пользователю.
    /// </summary>
    public class BroadcastResponse
    {
        /// <summary>
        /// Id трансляции.
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Название команды, которая играет дома.
        /// </summary>
        public string HomeTeam { get; set; }
        
        /// <summary>
        /// Название команды, которая играет в гостях.
        /// </summary>
        public string GuestTeam { get; set; }
        
        /// <summary>
        /// Время начала трансляции.
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// Количество голов, которые забила домашняя команда.
        /// </summary>
        public int? ScoreOfHomeTeam { get; set; }
        
        /// <summary>
        /// Количество голов, которые забила гостевая команда.
        /// </summary>
        public int? ScoreOfGuestTeam { get; set; }
        
        /// <summary>
        /// Текущий тайм.
        /// </summary>
        public int? CurrentHalf { get; set; }

        /// <summary>
        /// Текующее дополнительное игровое время тайма в минутах.
        /// </summary>
        public int? ExtraTime { get; set; }
        
        /// <summary>
        /// Общее дополнительное игровое время матча в минутах.
        /// </summary>
        public int? TotalExtraTime { get; set; }

        /// <summary>
        /// Статус трансляции.
        /// </summary>
        public BroadcastStatus Status { get; set; } = BroadcastStatus.NotStarted;
    }
}