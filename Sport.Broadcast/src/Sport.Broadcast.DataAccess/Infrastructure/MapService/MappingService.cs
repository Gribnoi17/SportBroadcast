using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.DataAccess.Broadcasts.Models;

namespace Sport.Broadcast.DataAccess.Infrastructure.MapService
{
    /// <summary>
    /// Класс для маппинга данных.
    /// </summary>
    internal static class MappingService
    {
        /// <summary>
        /// Маппит данные в модель для записи в базу данных.
        /// </summary>
        /// <param name="sportBroadcast">Данные спортивной трансляции.</param>
        /// <returns>Модель для записи в базу данных.</returns>
        public static BroadcastEntity MapToBroadcastEntity(SportBroadcast sportBroadcast)
        {
            return new BroadcastEntity()
            {
                HomeTeam = sportBroadcast.HomeTeam,
                GuestTeam = sportBroadcast.GuestTeam,
                StartTime = sportBroadcast.StartTime,
                Status = sportBroadcast.Status,
                CurrentHalf = sportBroadcast.CurrentHalf,
                ExtraTime = sportBroadcast.ExtraTime,
                TotalExtraTime = sportBroadcast.TotalExtraTime
            };
        }
        
        /// <summary>
        /// Маппит модель записи в базе данных в модель слоя бизнес логики. 
        /// </summary>
        /// <param name="broadcastEntity">Модель записи в базе данных.</param>
        /// <returns>Модель спортивной трансляции для слоя бизнес логики.</returns>
        public static SportBroadcast MapToSportBroadcast(BroadcastEntity broadcastEntity)
        {
            return new SportBroadcast()
            {
                Id = broadcastEntity.Id,
                HomeTeam = broadcastEntity.HomeTeam,
                GuestTeam = broadcastEntity.GuestTeam,
                StartTime = broadcastEntity.StartTime,
                ScoreOfHomeTeam = broadcastEntity.ScoreOfHomeTeam,
                ScoreOfGuestTeam = broadcastEntity.ScoreOfGuestTeam,
                Status = broadcastEntity.Status,
                CurrentHalf = broadcastEntity.CurrentHalf,
                ExtraTime = broadcastEntity.ExtraTime,
                TotalExtraTime = broadcastEntity.TotalExtraTime
            };
        }
        
        /// <summary>
        /// Маппит модель записи в базе данных в модель слоя бизнес логики. 
        /// </summary>
        /// <param name="broadcastEntity">Модель записи в базе данных.</param>
        /// <returns>Модель спортивной трансляции для слоя бизнес логики.</returns>
        public static void MapToUpdateBroadcastEntity(BroadcastEntity broadcastEntity, SportBroadcast sportBroadcast)
        {
            broadcastEntity.Status = sportBroadcast.Status;
            broadcastEntity.ScoreOfHomeTeam = sportBroadcast.ScoreOfHomeTeam;
            broadcastEntity.ScoreOfGuestTeam = sportBroadcast.ScoreOfGuestTeam;
            broadcastEntity.ExtraTime = sportBroadcast.ExtraTime;
            broadcastEntity.TotalExtraTime = sportBroadcast.TotalExtraTime;
            broadcastEntity.CurrentHalf = sportBroadcast.CurrentHalf;
        }
        
        /// <summary>
        /// Маппит модели записей в базе данных в список моделей слоя бизнес логики.
        /// </summary>
        /// <param name="sportBroadcastEntities">Список моделей записей в базе данных.</param>
        /// <returns>Список моделей слоя бизнес логики.</returns>
        public static List<SportBroadcast> MapToSportBroadcasts(List<BroadcastEntity> sportBroadcastEntities)
        {
            var broadcasts = new List<SportBroadcast>();

            foreach (var broadcastEntity in sportBroadcastEntities)
            {
                var broadcast = MapToSportBroadcast(broadcastEntity);
                broadcasts.Add(broadcast);
            }

            return broadcasts;
        }
    }
}