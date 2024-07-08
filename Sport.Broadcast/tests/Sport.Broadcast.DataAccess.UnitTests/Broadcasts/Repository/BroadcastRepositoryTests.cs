using Microsoft.EntityFrameworkCore;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.DataAccess.Broadcasts.Models;
using Sport.Broadcast.DataAccess.Broadcasts.Repository;
using Sport.Broadcast.DataAccess.Infrastructure.Data;
using Sport.Broadcast.DataAccess.UnitTests.Broadcasts.Data;
using Xunit;

namespace Sport.Broadcast.DataAccess.UnitTests.Broadcasts.Repository
{
    public class BroadcastRepositoryTests
    {
        private readonly SportBroadcastTestData _broadcastTestData;
        private readonly BroadcastEntityTestData _broadcastEntityTestData;
        private readonly DbContextOptions<BroadcastDbContext> _dbContextOptions;

        public BroadcastRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<BroadcastDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseForBroadcast" + Guid.NewGuid())
                .Options;

            _broadcastTestData = new SportBroadcastTestData();
            _broadcastEntityTestData = new BroadcastEntityTestData();
        }

        [Fact]
        public async Task RegisterSportBroadcast_WhenSuccessful_AddedBroadcastInDatabase()
        {
            //Assert
            var broadcast = _broadcastTestData.GetTestBroadcasts()[0];
            broadcast.Id = 1;

            long broadcastId;

            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);
                
                //Act
                broadcastId = await repository.RegisterSportBroadcast(broadcast, CancellationToken.None);
            }
            
            //Assert
            Assert.Equal(broadcast.Id, broadcastId);
        }
        
        [Fact]
        public async Task RegisterSportBroadcast_WhenNotSuccessful_ThrowsException()
        {
            //Assert
            var invalidEntity = new BroadcastEntity();
            var broadcast = _broadcastTestData.GetTestBroadcasts()[0];

            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);

                context.Entry(invalidEntity).State = EntityState.Added;
                
                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    repository.RegisterSportBroadcast(broadcast, CancellationToken.None));
            }
        }
        
        [Fact]
        public void RegisterSportBroadcast_WithCanceledToken_ReturnsCanceledTask()
        {
            //Arrange
            var canceledToken = new CancellationToken(canceled: true);
            var broadcast = _broadcastTestData.GetTestBroadcasts()[0];
            
            Task task;

            using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);

                task = repository.RegisterSportBroadcast(broadcast, canceledToken);
            }
            
            //Assert
            Assert.True(task.IsCanceled);
        }
        
        [Fact]
        public async Task GetSportBroadcasts_WhenSuccessful_ReturnsBroadcasts()
        {
            //Assert
            var broadcastEntities = _broadcastEntityTestData.GetTestBroadcastEntities();
            var expectedBroadcasts = _broadcastTestData.GetTestBroadcasts();
            expectedBroadcasts.RemoveRange(0, expectedBroadcasts.Count - 2);

            var date = new DateOnly(expectedBroadcasts[0].StartTime.Year, expectedBroadcasts[0].StartTime.Month, expectedBroadcasts[0].StartTime.Day);

            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                foreach (var broadcastEntity in broadcastEntities)
                {
                    await context.Broadcasts.AddAsync(broadcastEntity);
                }
                await context.SaveChangesAsync();
            }
            
            List<SportBroadcast> result;
            
            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);
                
                //Act
                result = await repository.GetSportBroadcasts(date, CancellationToken.None);
            }
            
            //Assert
            Assert.Equal(expectedBroadcasts.Count, result.Count);
            
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(expectedBroadcasts[i].HomeTeam, result[i].HomeTeam);
                Assert.Equal(expectedBroadcasts[i].GuestTeam, result[i].GuestTeam);
                Assert.Equal(expectedBroadcasts[i].StartTime, result[i].StartTime);
                Assert.Equal(expectedBroadcasts[i].Status, result[i].Status);
                Assert.Equal(expectedBroadcasts[i].ScoreOfHomeTeam, result[i].ScoreOfHomeTeam);
                Assert.Equal(expectedBroadcasts[i].ScoreOfGuestTeam, result[i].ScoreOfGuestTeam);
                Assert.Equal(expectedBroadcasts[i].CurrentHalf, result[i].CurrentHalf);
                Assert.Equal(expectedBroadcasts[i].ExtraTime, result[i].ExtraTime);
                Assert.Equal(expectedBroadcasts[i].TotalExtraTime, result[i].TotalExtraTime);
            }
        }
        
        [Fact]
        public async Task GetSportBroadcasts_WhenNotSuccessful_ThrowsException()
        {
            //Assert
            var date = new DateOnly(2026, 06, 06);

            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    repository.GetSportBroadcasts(date, CancellationToken.None));
            }
        }
        
        [Fact]
        public void GetSportBroadcasts_WithCanceledToken_ReturnsCanceledTask()
        {
            //Arrange
            var canceledToken = new CancellationToken(canceled: true);
            var date = new DateOnly(2024, 06, 06);
            
            Task task;

            using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);

                task = repository.GetSportBroadcasts(date, canceledToken);
            }
            
            //Assert
            Assert.True(task.IsCanceled);
        }
        
        [Fact]
        public async Task UpdateBroadcast_WhenSuccessful_UpdatedBroadcastInDatabase()
        {
            //Arrange
            var broadcastEntity = _broadcastEntityTestData.GetTestBroadcastEntities()[0];

            var updatedBroadcast = new SportBroadcast()
            {
                Id = 1,
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = new DateTime(2024, 08, 08, 12,00,00),
                ScoreOfHomeTeam = 1,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 1,
                ExtraTime = 5,
                TotalExtraTime = 5
            };
        
            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                await context.Broadcasts.AddAsync(broadcastEntity);
                await context.SaveChangesAsync();
            }
            
            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);
                
                //Act
                await repository.UpdateSportBroadcast(updatedBroadcast, CancellationToken.None);
            }
            
            // Assert
            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                var result = await context.Broadcasts.FindAsync(updatedBroadcast.Id);

                Assert.NotNull(result);
                Assert.Equal(updatedBroadcast.HomeTeam, result.HomeTeam);
                Assert.Equal(updatedBroadcast.GuestTeam, result.GuestTeam);
                Assert.Equal(updatedBroadcast.StartTime, result.StartTime);
                Assert.Equal(updatedBroadcast.Status, result.Status);
                Assert.Equal(updatedBroadcast.ScoreOfHomeTeam, result.ScoreOfHomeTeam);
                Assert.Equal(updatedBroadcast.ScoreOfGuestTeam, result.ScoreOfGuestTeam);
                Assert.Equal(updatedBroadcast.CurrentHalf, result.CurrentHalf);
                Assert.Equal(updatedBroadcast.ExtraTime, result.ExtraTime);
                Assert.Equal(updatedBroadcast.TotalExtraTime, result.TotalExtraTime);
            }
        }
        
        [Fact]
        public async Task UpdateBroadcast_WhenNotSuccessful_ThrowsException()
        {
            //Arrange
            var updatedBroadcast = new SportBroadcast()
            {
                Id = 1,
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = new DateTime(2024, 08, 08, 12,00,00),
                ScoreOfHomeTeam = 1,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 1,
                ExtraTime = 5,
                TotalExtraTime = 5
            };

            await using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);
                
                // Act & Assert
                await Assert.ThrowsAsync<Exception>(() =>
                    repository.UpdateSportBroadcast(updatedBroadcast, CancellationToken.None));
            }
        }
        
        [Fact]
        public void UpdateBroadcast__WithCanceledToken_ReturnsCanceledTask()
        {
            //Arrange
            var canceledToken = new CancellationToken(canceled: true);
            
            var updatedBroadcast = new SportBroadcast()
            {
                Id = 1,
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = new DateTime(2024, 08, 08, 12,00,00),
                ScoreOfHomeTeam = 1,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 1,
                ExtraTime = 5,
                TotalExtraTime = 5
            };
            
            Task task;

            using (var context = new BroadcastDbContext(_dbContextOptions))
            {
                IBroadcastRepository repository = new BroadcastRepository(context);

                task = repository.UpdateSportBroadcast(updatedBroadcast, canceledToken);
            }
            
            //Assert
            Assert.True(task.IsCanceled);
        }
    }
}