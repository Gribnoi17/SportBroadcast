using Microsoft.EntityFrameworkCore;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.DataAccess.Infrastructure.Data;
using Sport.Broadcast.DataAccess.Infrastructure.MapService;

namespace Sport.Broadcast.DataAccess.Broadcasts.Repository
{
    /// <inheritdoc cref="IBroadcastRepository" />
    internal class BroadcastRepository : IBroadcastRepository
    {
        private readonly BroadcastDbContext _context;
        
        /// <summary>
        /// Инициализирует объект класса SportBroadcastRepository.
        /// </summary>
        /// <param name="context">Контекст для работы с базой данных.</param>
        public BroadcastRepository(BroadcastDbContext context)
        {
            _context = context;
        }

        public async Task<long> RegisterSportBroadcast(SportBroadcast sportBroadcast, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<long>(token);
            }
            
            var broadcastEntity = MappingService.MapToBroadcastEntity(sportBroadcast);

            try
            {
                var result = await _context.Broadcasts.AddAsync(broadcastEntity, token);

                await _context.SaveChangesAsync(token);
                
                return result.Entity.Id;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"При добавлении трансляции что-то пошло не так: {e.Message}");
            }
        }

        public async Task UpdateSportBroadcast(SportBroadcast sportBroadcast, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                await Task.FromCanceled(token);
                return;
            }

            try
            {
                var broadcastsEntity = await _context.Broadcasts.FirstOrDefaultAsync(b => b.Id == sportBroadcast.Id, token);

                if (broadcastsEntity == null)
                {
                    throw new ArgumentException($"Трансляция с таким Id: {sportBroadcast.Id} не найдена!");
                }
                
                MappingService.MapToUpdateBroadcastEntity(broadcastsEntity, sportBroadcast);

                await _context.SaveChangesAsync(token);
            }
            catch (Exception e)
            {
                throw new Exception($"При обновлении трансляции с Id: {sportBroadcast.Id} что-то пошло нет так. Ошибка: {e.Message}");
            }
        }

        public async Task<List<SportBroadcast>> GetSportBroadcasts(DateOnly dateBroadcast, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<List<SportBroadcast>>(token);
            }
            
            var startOfDay = new DateTime(dateBroadcast.Year, dateBroadcast.Month, dateBroadcast.Day, 0, 0, 0, 001, DateTimeKind.Utc);
            var endOfDay = new DateTime(dateBroadcast.Year, dateBroadcast.Month, dateBroadcast.Day, 23, 59, 59, 999, DateTimeKind.Utc);

            var broadcastsEntities = _context.Broadcasts
                .AsNoTracking()
                .Where(b => b.StartTime >= startOfDay && b.StartTime <= endOfDay)
                .ToList();

            if (broadcastsEntities.Count == 0)
            {
                throw new InvalidOperationException($"Трансляций на дату: {dateBroadcast} не найдено!");
            }

            var broadcasts = MappingService.MapToSportBroadcasts(broadcastsEntities);
            
            return broadcasts;
        }
        
        public async Task<SportBroadcast> GetSportBroadcast(long broadcastId, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                return await Task.FromCanceled<SportBroadcast>(token);
            }

            var broadcastsEntity = await _context.Broadcasts.FirstOrDefaultAsync(b => b.Id == broadcastId, token);

            if (broadcastsEntity == null)
            {
                throw new InvalidOperationException($"Трансляция с Id: {broadcastId} не найдена!");
            }

            var broadcast = MappingService.MapToSportBroadcast(broadcastsEntity);
            
            return broadcast;
        }
    }
}