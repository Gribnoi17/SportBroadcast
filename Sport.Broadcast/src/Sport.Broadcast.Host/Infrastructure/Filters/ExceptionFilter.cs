using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Sport.Broadcast.AppServices.Contract.Infrastructure.Exceptions;

namespace Sport.Broadcast.Host.Infrastructure.Filters
{
    /// <summary>
    /// Фильтр исключений для обработки и преобразования исключений в соответствующие HTTP-ответы.
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Обработка исключений и преобразование их в соответствующие HTTP-ответы.
        /// </summary>
        /// <param name="context">Контекст исключения.</param>
        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is ValidationException validationException)
                {
                    context.Result = new BadRequestObjectResult(validationException.Message);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is InvalidOperationException operationException)
                {
                    context.Result = new NotFoundObjectResult(operationException.Message);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is FormatException formatException)
                {
                    context.Result = new BadRequestObjectResult(formatException.Message);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is HubException hubException)
                {
                    context.Result = new BadRequestObjectResult(hubException.Message);
                    context.ExceptionHandled = true;
                }
                else if (context.Exception is Exception exception)
                {
                    context.Result = new NotFoundObjectResult(exception.Message);
                    context.ExceptionHandled = true;
                }
                
                _logger.LogError(context.Exception.Message);
            }
        }
    }
}