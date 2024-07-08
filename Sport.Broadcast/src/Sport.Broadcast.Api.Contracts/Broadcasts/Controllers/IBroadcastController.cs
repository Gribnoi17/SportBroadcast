using Sport.Broadcast.Api.Contracts.Broadcasts.Requests;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;

namespace Sport.Broadcast.Api.Contracts.Broadcasts.Controllers
{
    /// <summary>
    /// Предназначен для работы с трансляцией.
    /// </summary>
    public interface IBroadcastController
    {
        /// <summary>
        /// Регистрация трансляции.
        /// </summary>
        /// <param name="request">Данные для регистрации трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Зарегистрированную трансляцию.</returns>
        Task<BroadcastResponse> RegisterBroadcast(BroadcastRegistrationRequest request, CancellationToken token);
        
        /// <summary>
        /// Получение списка всех трансляция на заданную дату.
        /// </summary>
        /// <param name="date">Дата трансляции.</param>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Список трансляций.</returns>
        public Task<BroadcastResponse[]> GetBroadcasts(DateOnly date, CancellationToken token);
    }
}