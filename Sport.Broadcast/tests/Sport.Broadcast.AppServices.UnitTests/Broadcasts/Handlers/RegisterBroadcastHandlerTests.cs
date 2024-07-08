using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sport.Broadcast.AppServices.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Validators;
using Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data;
using Xunit;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Handlers
{
    public class RegisterBroadcastHandlerTests
    {
        private readonly IBroadcastRepository _repository;
        private readonly IBroadcastValidator _validator;
        private readonly IRegisterBroadcastHandler _handler;
        private readonly ILogger<RegisterBroadcastHandler> _logger;

        public RegisterBroadcastHandlerTests()
        {
            _repository = Substitute.For<IBroadcastRepository>();
            _validator = Substitute.For<IBroadcastValidator>();
            _logger = Substitute.For<ILogger<RegisterBroadcastHandler>>();
            
            _handler = new RegisterBroadcastHandler(_repository, _validator, _logger);
        }

        [Theory]
        [ClassData(typeof(ValidBroadcastInternalRequestTestData))]
        public async Task Handle_SuccessfulBroadcastCreation_ReturnsBroadcast(BroadcastInternalRequest request)
        {
            //Arrange
            long nextId = 0;
            
            var broadcast = new SportBroadcast()
            {
                HomeTeam = request.HomeTeam,
                GuestTeam = request.GuestTeam,
                StartTime = request.StartTime
            };
            
            _repository.RegisterSportBroadcast(Arg.Any<SportBroadcast>(), CancellationToken.None).Returns(x => 
            {
                var broadcastArgument = x.Arg<SportBroadcast>();
                broadcastArgument.Id = ++nextId;
                broadcast.Id = nextId;
                return broadcast.Id;
            });
            
            //Act
            var result = await _handler.Handle(request, CancellationToken.None);
            
            //Assert
            Assert.Equal(broadcast.Id, result.Id);
            Assert.Equal(broadcast.HomeTeam, result.HomeTeam);
            Assert.Equal(broadcast.GuestTeam, result.GuestTeam);
            Assert.Equal(broadcast.StartTime, result.StartTime);
        }
        
        [Theory]
        [ClassData(typeof(ValidBroadcastInternalRequestTestData))]
        public async Task Handle_NotSuccessfulBroadcastCreation_ThrowsInvalidOperationException(BroadcastInternalRequest request)
        {
            //Arrange
            _repository.RegisterSportBroadcast(Arg.Any<SportBroadcast>(), CancellationToken.None).Throws(new InvalidOperationException());
            
            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(new BroadcastInternalRequest(), CancellationToken.None)
            );
        }
    }
}