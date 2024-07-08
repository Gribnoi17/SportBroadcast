using Microsoft.Extensions.Logging;
using NSubstitute;
using Sport.Broadcast.AppServices.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;
using Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data;
using Xunit;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Handlers
{
    public class SendMessageHandlerTests
    {
        private readonly IBroadcastRepository _repository;
        private readonly ISendMessageHandler _sendMessageHandler;
        private readonly ILogger<SendMessageHandler> _logger;

        public SendMessageHandlerTests()
        {
            _repository = Substitute.For<IBroadcastRepository>();
            _logger = Substitute.For<ILogger<SendMessageHandler>>();

            _sendMessageHandler = new SendMessageHandler(_repository, _logger);
        }

        [Theory]
        [ClassData(typeof(SportBroadcastTestData))]
        public async Task Handle_ShouldUpdateBroadcast_WhenEventIsNotNull(SportBroadcast sportBroadcast)
        {
            // Arrange
            var message = new MessageInternalRequest { Event = EventInGame.Goal, Score = "2-0" };
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(sportBroadcast);

            // Act
            await _sendMessageHandler.Handle(sportBroadcast.Id, message);

            // Assert
            await _repository.Received(1).UpdateSportBroadcast(sportBroadcast, Arg.Any<CancellationToken>());
        }

        [Theory]
        [ClassData(typeof(SportBroadcastTestData))]
        public async Task Handle_ShouldNotUpdateBroadcast_WhenEventIsNull(SportBroadcast sportBroadcast)
        {
            // Arrange
            var message = new MessageInternalRequest { Event = null };
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(sportBroadcast);

            // Act
            await _sendMessageHandler.Handle(sportBroadcast.Id, message);

            // Assert
            await _repository.DidNotReceive().UpdateSportBroadcast(sportBroadcast, Arg.Any<CancellationToken>());
        }

        [Fact]
        public void ShouldUpdateBroadcast_ShouldReturnTrue_WhenEventIsUpdateable()
        {
            // Arrange
            var eventInGame = EventInGame.Goal;

            // Act
            var result = SendMessageHandler.ShouldUpdateBroadcast(eventInGame);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldUpdateBroadcast_ShouldReturnFalse_WhenEventIsNotUpdateable()
        {
            // Arrange
            var eventInGame = EventInGame.YellowCard;

            // Act
            var result = SendMessageHandler.ShouldUpdateBroadcast(eventInGame);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateExtraTime_ShouldUpdateBroadcastCorrectly_UpdateSuccessfully()
        {
            // Arrange
            var broadcast = new SportBroadcast();
            var message = new MessageInternalRequest { ExtraTime = 10 };

            var expectedResult = 10;

            // Act
            SendMessageHandler.UpdateExtraTime(broadcast, message);

            // Assert
            Assert.Equal(expectedResult, broadcast.ExtraTime);
            Assert.Equal(expectedResult, broadcast.TotalExtraTime);
        }

        [Fact]
        public void UpdateBreak_ShouldUpdateBroadcastCorrectly_UpdateSuccessfully()
        {
            // Arrange
            var broadcast = new SportBroadcast();
            var message = new MessageInternalRequest();

            var expectedResult = 0;

            // Act
            SendMessageHandler.UpdateBreak(broadcast, message);

            // Assert
            Assert.Equal(expectedResult, broadcast.CurrentHalf);
            Assert.Equal(expectedResult, broadcast.ExtraTime);
        }

        [Fact]
        public void UpdateAfterBreak_ShouldUpdateBroadcastCorrectly_UpdateSuccessfully()
        {
            // Arrange
            var broadcast = new SportBroadcast();
            var message = new MessageInternalRequest { HalfNumberAfterBreak = 2 };

            var expectedResult = 2;

            // Act
            SendMessageHandler.UpdateAfterBreak(broadcast, message);

            // Assert
            Assert.Equal(expectedResult, broadcast.CurrentHalf);
        }

        [Fact]
        public void UpdateGoal_ShouldUpdateBroadcastCorrectly_UpdateSuccessfully()
        {
            // Arrange
            var broadcast = new SportBroadcast();
            var message = new MessageInternalRequest { Score = "3-1" };

            var scoreOfHomeTeam = 3;
            var scoreOfGuestTeam = 1;

            // Act
            SendMessageHandler.UpdateGoal(broadcast, message);

            // Assert
            Assert.Equal(scoreOfHomeTeam, broadcast.ScoreOfHomeTeam);
            Assert.Equal(scoreOfGuestTeam, broadcast.ScoreOfGuestTeam);
        }

        [Fact]
        public void UpdateGoal_ShouldThrowException_ReturnException()
        {
            // Arrange
            var broadcast = new SportBroadcast();
            var message = new MessageInternalRequest();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => SendMessageHandler.UpdateGoal(broadcast, message));
        }
    }
}