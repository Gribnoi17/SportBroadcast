namespace Sport.Broadcast.Api.Contracts.Broadcasts.Requests
{
    /// <summary>
    /// Данные, для регистарции трансляции.
    /// </summary>
    public record BroadcastRegistrationRequest
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