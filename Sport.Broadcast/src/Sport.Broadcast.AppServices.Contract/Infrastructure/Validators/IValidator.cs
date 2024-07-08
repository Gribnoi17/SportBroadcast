namespace Sport.Broadcast.AppServices.Contract.Infrastructure.Validators
{
    /// <summary>
    /// Валидатор для проверки входных данных.
    /// </summary>
    /// <typeparam name="T">Тип модели, подлежащей валидации.</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Проверяет переданные данные на соответствие определенным правилам валидации.
        /// </summary>
        /// <param name="request">Данные, подлежащая валидации.</param>
        void Validate(T request);
    }
}