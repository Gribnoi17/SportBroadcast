using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sport.Broadcast.AppServices.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.Broadcasts.Repository;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Handlers;
using Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data;
using Xunit;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Handlers
{
    public class SearchBroadcastHandlerTests
    {
        private readonly SportBroadcastTestData _testData;
        private readonly IBroadcastRepository _repository;
        private readonly ISearchBroadcastHandler _handler;
        private readonly ILogger<SearchBroadcastHandler> _logger;

        public SearchBroadcastHandlerTests()
        {
            _testData = new SportBroadcastTestData();
            _repository = Substitute.For<IBroadcastRepository>();
            _logger = Substitute.For<ILogger<SearchBroadcastHandler>>();
            
            _handler = new SearchBroadcastHandler(_repository, _logger);
        }

        [Fact]
        public async Task Handle_BroadcastsAreInDatabase_ReturnsBroadcasts()
        {
            //Assert
            var broadcasts = _testData.GetBroadcasts();
            foreach (var broadcast in broadcasts)
            {
                broadcast.StartTime = new DateTime(2024, 08, 17);
            }    
            
            var testDate = new DateOnly(broadcasts[0].StartTime.Year, broadcasts[0].StartTime.Month, broadcasts[0].StartTime.Day);

            _repository.GetSportBroadcasts(testDate, CancellationToken.None).Returns(broadcasts);
            
            //Act
            var result = await _handler.Handle(testDate, CancellationToken.None);
            
            //Assert
            Assert.Equal(broadcasts, result);
        }
        
        [Fact]
        public async Task Handle_BroadcastsAreNotInDatabase_ThrowsInvalidOperationException()
        {
            //Assert
            var testDate = new DateOnly(2024, 06, 05);

            _repository.GetSportBroadcasts(testDate, CancellationToken.None).Throws(new InvalidOperationException());

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(testDate, CancellationToken.None));
        }
    }
}