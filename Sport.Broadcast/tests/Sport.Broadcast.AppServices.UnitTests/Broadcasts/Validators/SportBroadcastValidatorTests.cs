using Sport.Broadcast.AppServices.Broadcasts.Validators;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Validators;
using Sport.Broadcast.AppServices.Contract.Infrastructure.Exceptions;
using Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data;
using Xunit;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Validators
{
    public class SportBroadcastValidatorTests
    {
        private readonly IBroadcastValidator _validator = new BroadcastValidator();

        [Theory]
        [ClassData(typeof(ValidBroadcastInternalRequestTestData))]
        public void Validate_ValidInput_NoExceptionThrown(BroadcastInternalRequest request)
        {
            //Arrange
            
            //Act
            var exception = Record.Exception(() => _validator.Validate(request));
            
            //Assert
            Assert.Null(exception);
        }
        
        [Theory]
        [ClassData(typeof(InvalidBroadcastInternalRequestTestData))]
        public void Validate_InvalidInput_ValidationExceptionThrown(BroadcastInternalRequest request)
        {
            //Arrange

            //Act & Assert
            Assert.Throws<ValidationException>(() => _validator.Validate(request));
        }
    }
}