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
    public class JoinBroadcastHandlerTests
    {
        private readonly IBroadcastRepository _repository;
        private readonly IJoinBroadcastHandler _joinBroadcastHandler;
        private readonly ILogger<JoinBroadcastHandler> _logger;

        public JoinBroadcastHandlerTests()
        {
            _repository = Substitute.For<IBroadcastRepository>();
            _logger = Substitute.For<ILogger<JoinBroadcastHandler>>();

            _joinBroadcastHandler = new JoinBroadcastHandler(_repository, _logger);
        }

        [Theory]
        [ClassData(typeof(SportBroadcastTestData))]
        public async Task Handle_BroadcastStarted_ReturnTrue(Contract.Broadcasts.Models.SportBroadcast sportBroadcast)
        {
            //Arrange
            sportBroadcast.Status = BroadcastStatus.Started;
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(sportBroadcast);
            
            //Act
            var result = await _joinBroadcastHandler.Handle(sportBroadcast.Id, CancellationToken.None);
            
            //Assert
            Assert.True(result);
        }
        
        [Theory]
        [ClassData(typeof(SportBroadcastTestData))]
        public async Task Handle_BroadcastEnded_ReturnFalse(Contract.Broadcasts.Models.SportBroadcast sportBroadcast)
        {
            //Arrange
            sportBroadcast.Status = BroadcastStatus.Ended;
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(sportBroadcast);
            
            //Act
            var result = await _joinBroadcastHandler.Handle(sportBroadcast.Id, CancellationToken.None);
            
            //Assert
            Assert.False(result);
        }
        
        [Theory]
        [ClassData(typeof(SportBroadcastTestData))]
        public async Task Handle_BroadcastNotStarted_ReturnFalse(Contract.Broadcasts.Models.SportBroadcast sportBroadcast)
        {
            //Arrange
            sportBroadcast.Status = BroadcastStatus.NotStarted;
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Returns(sportBroadcast);
            
            //Act
            var result = await _joinBroadcastHandler.Handle(sportBroadcast.Id, CancellationToken.None);
            
            //Assert
            Assert.False(result);
        }
        
        [Fact]
        public async Task Handle_BroadcastNotInDatabase_ThrowsInvalidOperationException()
        {
            //Arrange
            long broadcastId = -1;
            
            _repository.GetSportBroadcast(Arg.Any<long>(), CancellationToken.None).Throws(new InvalidOperationException());
            
            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _joinBroadcastHandler.Handle(broadcastId, CancellationToken.None));
        }
    }
}