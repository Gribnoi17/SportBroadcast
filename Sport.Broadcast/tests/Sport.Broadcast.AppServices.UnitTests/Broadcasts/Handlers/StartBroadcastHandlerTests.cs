using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sport.Broadcast.AppServices.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Xunit;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Handlers
{
    public class StartBroadcastHandlerTests
    {
        private readonly IBroadcastRepository _repository;
        private readonly IStartBroadcastHandler _startBroadcastHandler;
        private readonly ILogger<StartBroadcastHandler> _logger;
        
        public StartBroadcastHandlerTests()
        {
            _repository = Substitute.For<IBroadcastRepository>();
            _logger = Substitute.For<ILogger<StartBroadcastHandler>>();

            _startBroadcastHandler = new StartBroadcastHandler(_repository, _logger);
        }
        
        [Fact]
        public async Task Handle_BroadcastStartedSuccessfully_UpdatedBroadcastFields()
        {
            //Arrange
            var broadcast = new SportBroadcast()
            {
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = DateTime.Now.ToLocalTime(),
                ScoreOfHomeTeam = 0,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 2, ExtraTime = 0,
                TotalExtraTime = 3,
                Status = BroadcastStatus.Started
            };
            
            var expectedBroadcast = new SportBroadcast()
            {
                Id = broadcast.Id,
                HomeTeam = broadcast.HomeTeam,
                GuestTeam = broadcast.GuestTeam,
                StartTime = broadcast.StartTime,
                Status = BroadcastStatus.Started,
                ScoreOfHomeTeam = 0,
                ScoreOfGuestTeam = 0,
                ExtraTime = 0,
                TotalExtraTime = 0,
                CurrentHalf = 1,
            };
            
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(broadcast);
            
            //Act
            var result = await _startBroadcastHandler.Handle(broadcast.Id, CancellationToken.None);
            
            //Assert
            Assert.True(result.Item2);
            Assert.Equal(expectedBroadcast.HomeTeam, result.Item1.HomeTeam);
            Assert.Equal(expectedBroadcast.GuestTeam, result.Item1.GuestTeam);
            Assert.Equal(expectedBroadcast.StartTime, result.Item1.StartTime);
            Assert.Equal(expectedBroadcast.Status, result.Item1.Status);
            Assert.Equal(expectedBroadcast.ScoreOfHomeTeam, result.Item1.ScoreOfHomeTeam);
            Assert.Equal(expectedBroadcast.ScoreOfGuestTeam, result.Item1.ScoreOfGuestTeam);
            Assert.Equal(expectedBroadcast.CurrentHalf, result.Item1.CurrentHalf);
            Assert.Equal(expectedBroadcast.ExtraTime, result.Item1.ExtraTime);
            Assert.Equal(expectedBroadcast.TotalExtraTime, result.Item1.TotalExtraTime);
        }
        
        [Fact]
        public async Task Handle_BroadcastStartedNotSuccessfully_UpdatedBroadcastFields()
        {
            //Arrange
            var broadcast = new SportBroadcast()
            {
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = (DateTime.Now + TimeSpan.FromMinutes(2)).ToLocalTime(),
                ScoreOfHomeTeam = 0,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 2,
                ExtraTime = 0,
                TotalExtraTime = 3,
                Status = BroadcastStatus.Started
            };
            
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(broadcast);
            
            //Act
            var result = await _startBroadcastHandler.Handle(broadcast.Id, CancellationToken.None);
            
            //Assert
            Assert.False(result.Item2);
        }
        
        [Fact]
        public async Task Handle_BroadcastNotInDatabase_ThrowsInvalidOperationException()
        {
            //Arrange
            long broadcastId = -1;
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Throws(new InvalidOperationException());
            
            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _startBroadcastHandler.Handle(broadcastId, CancellationToken.None));
        }
    }
}