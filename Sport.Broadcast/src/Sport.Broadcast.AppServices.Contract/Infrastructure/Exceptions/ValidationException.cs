namespace Sport.Broadcast.AppServices.Contract.Infrastructure.Exceptions
{
    /// <summary>
    /// Обрабочтик ошибок, который отлавливает ошибки при валидации данных.
    /// </summary>
    public class ValidationException : Exception
    {
        private readonly string _message = string.Empty;

        /// <summary>
        /// Инициализирует экземпляр класса ValidationException.
        /// </summary>
        /// <param name="errors">Список ошибок, возникших в процессе валидации.</param>
        public ValidationException(List<string> errors)
        {
            foreach (var error in errors)
            {
                _message += error + " ";
            }
        }

        /// <summary>
        /// Сообщение об исключении, содержащее список ошибок валидации.
        /// </summary>
        public override string Message => _message;
    }
}