using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Validators;
using Sport.Broadcast.AppServices.Contract.Infrastructure.Exceptions;

namespace Sport.Broadcast.AppServices.Broadcasts.Validators
{
    /// <inheritdoc cref="IBroadcastValidator"/>
    internal class BroadcastValidator : IBroadcastValidator
    {
        public void Validate(BroadcastInternalRequest request)
        {
            var validationExceptions = new List<string>();

            if (string.IsNullOrWhiteSpace(request.GuestTeam))
            {
                validationExceptions.Add("Не указана команда, которая будет играть в гостях.");
            }

            if (string.IsNullOrWhiteSpace(request.HomeTeam))
            {
                validationExceptions.Add("Не указана команда, которая будет играть дома.");
            }

            if (request.StartTime.ToUniversalTime() < DateTime.UtcNow)
            {
                validationExceptions.Add("Время начала трансляции указано в прошлом.");
            }

            if (validationExceptions.Count > 0)
            {
                throw new ValidationException(validationExceptions);
            }
        }
    }
}