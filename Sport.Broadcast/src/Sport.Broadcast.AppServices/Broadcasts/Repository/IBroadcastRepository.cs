using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.Broadcasts.Repository
{
    /// <summary>
    /// Репозиторий, который работает с базой данных спортивных трансляций.
    /// </summary>
    public interface IBroadcastRepository
    {
        /// <summary>
        /// Добавить новую запись спортивной трансляции в базу данных.
        /// </summary>
        /// <param name="sportBroadcast">Данные спортивной трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Id добавленной записи.</returns>
        public Task<long> RegisterSportBroadcast(SportBroadcast sportBroadcast, CancellationToken token);
        
        /// <summary>
        /// Обновить данные записи спортивной трансляции в базе данных.
        /// </summary>
        /// <param name="sportBroadcast">Обновленные данные.</param>
        /// <param name="token">Токе отмены операции.</param>
        /// <returns></returns>
        public Task UpdateSportBroadcast(SportBroadcast sportBroadcast, CancellationToken token);
        
        /// <summary>
        /// Получение всех записей спортивных трансляций из базы данных на определенную дату.
        /// </summary>
        /// <param name="dateBroadcast">Дата спортивной/ых трансляций.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Список спортивных трансляций на определенную дату.</returns>
        public Task<List<SportBroadcast>> GetSportBroadcasts(DateOnly dateBroadcast, CancellationToken token);
        
        /// <summary>
        /// Получение спортивной трансляции из базы данных по Id трансляции.
        /// </summary>
        /// <param name="broadcastId">Id трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Спортивная трансляция с определенным Id.</returns>
        public Task<SportBroadcast> GetSportBroadcast(long broadcastId, CancellationToken token);
    }
}