namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Models
{
    /// <summary>
    /// Модель запроса слоя бизнес лоигки для регстрации трансляции.
    /// </summary>
    public class BroadcastInternalRequest
    {
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
    }
}