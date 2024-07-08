using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers
{
    /// <summary>
    /// Обработчик для поиска трансляций.
    /// </summary>
    public interface ISearchBroadcastHandler
    {
        /// <summary>
        /// Возвращает трансляции, которые есть на определенную дату.
        /// </summary>
        /// <param name="date">Дата трансляции.</param>
        /// <param name="token">Токен для отмены операции.</param>
        /// <returns>Список трансляций.</returns>
        Task<List<SportBroadcast>> Handle(DateOnly date, CancellationToken token);
    }
}