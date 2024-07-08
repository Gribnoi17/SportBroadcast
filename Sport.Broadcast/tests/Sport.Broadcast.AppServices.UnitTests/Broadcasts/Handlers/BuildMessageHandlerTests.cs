using Microsoft.Extensions.Logging;
using NSubstitute;
using Sport.Broadcast.AppServices.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;
using Xunit;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Handlers
{
    public class BuildMessageHandlerTests
    {
        private readonly IBuildMessageHandler _buildMessageHandler;
        private readonly ILogger<BuildMessageHandler> _logger;

        public BuildMessageHandlerTests()
        {
            _logger = Substitute.For<ILogger<BuildMessageHandler>>();

            _buildMessageHandler = new BuildMessageHandler(_logger);
        }
        
        [Fact]
        public async Task Handle_Should_BuildCorrectMessage()
        {
            // Arrange
            var expectedResult = "15;Гол;2-1;Андрей Логинов;Забили мяч!";
            var message = new MessageInternalRequest
            {
                Minute = "15",
                Event = EventInGame.Goal,
                Score = "2-1",
                PlayerName = "Андрей Логинов",
                Text = "Забили мяч!"
            };

            // Act
            var result = await _buildMessageHandler.Handle(message);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void BuildStringResult_ShouldAppendMinute_WhenMessageHasMinute()
        {
            // Arrange
            var message = new MessageInternalRequest { Minute = "10" };
            var expectedResult = "10;";

            // Act
            var result = BuildMessageHandler.BuildStringResult(message);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BuildStringResult_ShouldAppendEventDescription_WhenMessageHasEvent()
        {
            // Arrange
            var message = new MessageInternalRequest { Minute = "10", Event = EventInGame.Goal };

            var expectedResult = "10;Гол;";

            // Act
            var result = BuildMessageHandler.BuildStringResult(message);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BuildStringResult_ShouldAppendScore_WhenMessageHasScore()
        {
            // Arrange
            var message = new MessageInternalRequest { Minute = "10", Event = EventInGame.Goal, Score = "1-0" };

            var expectedResult = "10;Гол;1-0;";

            // Act
            var result = BuildMessageHandler.BuildStringResult(message);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BuildStringResult_ShouldAppendPlayerName_WhenMessageHasPlayerName()
        {
            // Arrange
            var message = new MessageInternalRequest
            {
                Minute = "10",
                Event = EventInGame.Goal,
                Score = "1-0",
                PlayerName = "Андрей"
            };
            var expectedResult = "10;Гол;1-0;Андрей;";

            // Act
            var result = BuildMessageHandler.BuildStringResult(message);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BuildStringResult_ShouldAppendText_WhenMessageHasText()
        {
            // Arrange
            var message = new MessageInternalRequest
            {
                Minute = "10",
                Event = EventInGame.Goal,
                Score = "1-0",
                PlayerName = "Андрей",
                Text = "Андрей забил гол в ворота!"
            };

            var expectedResult = "10;Гол;1-0;Андрей;Андрей забил гол в ворота!";

            // Act
            var result = BuildMessageHandler.BuildStringResult(message);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetEventDescription_ShouldReturnCorrectDescription_ReturnCorrectDescription()
        {
            // Arrange
            var eventInGame = EventInGame.Goal;
            var expectedResult = "Гол";

            // Act
            var result = BuildMessageHandler.GetEventDescription(eventInGame);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}