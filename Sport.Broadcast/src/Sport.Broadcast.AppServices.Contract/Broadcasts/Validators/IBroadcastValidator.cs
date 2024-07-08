using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Infrastructure.Validators;

namespace Sport.Broadcast.AppServices.Contract.Broadcasts.Validators
{
    /// <summary>
    /// Валидатор для проверки входных данных для регистрации трансляции.
    /// </summary>
    public interface IBroadcastValidator : IValidator<BroadcastInternalRequest>
    {

    }
}