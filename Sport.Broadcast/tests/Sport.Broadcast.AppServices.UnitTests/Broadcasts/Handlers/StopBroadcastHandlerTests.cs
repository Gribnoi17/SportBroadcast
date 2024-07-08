using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sport.Broadcast.AppServices.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data;
using Xunit;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Handlers
{
    public class StopBroadcastHandlerTests
    {
        private readonly IBroadcastRepository _repository;
        private readonly IStopBroadcastHandler _stopBroadcastHandler;
        private readonly ILogger<StopBroadcastHandler> _logger;

        public StopBroadcastHandlerTests()
        {
            _repository = Substitute.For<IBroadcastRepository>();
            _logger = Substitute.For<ILogger<StopBroadcastHandler>>();
            
            _stopBroadcastHandler = new StopBroadcastHandler(_repository, _logger);
        }

        [Theory]
        [ClassData(typeof(SportBroadcastTestData))]
        public async Task Handle_BroadcastStoppedSuccessfully_UpdatedBroadcastFields(SportBroadcast sportBroadcast)
        {
            //Arrange
            var expectedBroadcast = new Contract.Broadcasts.Models.SportBroadcast()
            {
                Id = sportBroadcast.Id,
                HomeTeam = sportBroadcast.HomeTeam,
                GuestTeam = sportBroadcast.GuestTeam,
                StartTime = sportBroadcast.StartTime,
                ScoreOfHomeTeam = sportBroadcast.ScoreOfHomeTeam,
                ScoreOfGuestTeam = sportBroadcast.ScoreOfGuestTeam,
                TotalExtraTime = sportBroadcast.TotalExtraTime,
                Status = BroadcastStatus.Ended,
                CurrentHalf = 0,
                ExtraTime = 0,
            };
            
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(sportBroadcast);
            
            //Act
            await _stopBroadcastHandler.Handle(sportBroadcast.Id, CancellationToken.None);
            
            //Assert
            Assert.Equal(expectedBroadcast.HomeTeam, sportBroadcast.HomeTeam);
            Assert.Equal(expectedBroadcast.GuestTeam, sportBroadcast.GuestTeam);
            Assert.Equal(expectedBroadcast.StartTime, sportBroadcast.StartTime);
            Assert.Equal(expectedBroadcast.Status, sportBroadcast.Status);
            Assert.Equal(expectedBroadcast.ScoreOfHomeTeam, sportBroadcast.ScoreOfHomeTeam);
            Assert.Equal(expectedBroadcast.ScoreOfGuestTeam, sportBroadcast.ScoreOfGuestTeam);
            Assert.Equal(expectedBroadcast.CurrentHalf, sportBroadcast.CurrentHalf);
            Assert.Equal(expectedBroadcast.ExtraTime, sportBroadcast.ExtraTime);
            Assert.Equal(expectedBroadcast.TotalExtraTime, sportBroadcast.TotalExtraTime);
        }
        
        [Fact]
        public async Task Handle_BroadcastNotInDatabase_ThrowsInvalidOperationException()
        {
            //Arrange
            long broadcastId = -1;
            
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Throws(new InvalidOperationException());
            
            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _stopBroadcastHandler.Handle(broadcastId, CancellationToken.None));
        }
    }
}