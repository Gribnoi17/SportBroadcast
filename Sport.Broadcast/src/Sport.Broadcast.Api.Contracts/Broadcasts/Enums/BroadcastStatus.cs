using System.Text.Json.Serialization;

namespace Sport.Broadcast.Api.Contracts.Broadcasts.Enums
{
    /// <summary>
    /// Статус спортивной трансляции.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]  
    public enum BroadcastStatus
    {
        /// <summary>
        /// Не началась.
        /// </summary>
        NotStarted,
        
        /// <summary>
        /// Запущена.
        /// </summary>
        Started,
        
        /// <summary>
        /// Закончена.
        /// </summary>
        Ended
    }
}