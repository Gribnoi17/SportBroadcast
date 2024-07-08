using Microsoft.EntityFrameworkCore;
using Sport.Broadcast.DataAccess.Broadcasts.Models;

namespace Sport.Broadcast.DataAccess.Infrastructure.Data
{
    /// <summary>
    /// Контекст базы данных споривных трансляций.
    /// </summary>
    internal class BroadcastDbContext : DbContext
    {
        /// <summary>
        /// DbSet спортивных трансляций.
        /// </summary>
        public DbSet<BroadcastEntity> Broadcasts { get; set; }
        
        /// <summary>
        /// Инициализирует объект класса SportBroadcastDbContext.
        /// </summary>
        /// <param name="options">Настройки для контекста базы данных.</param>
        public BroadcastDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}