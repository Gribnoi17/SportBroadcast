using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sport.Broadcast.Host.Infrastructure.Binders
{
    /// <summary>
    /// Биндер для привязки модели типа DateOnly с определенным форматом даты.
    /// </summary>
    public class DateOnlyModelBinder : IModelBinder
    {
        private const string _formatDate = "yyyy-MM-dd";
        
        /// <summary>
        /// Привязки модели.
        /// </summary>
        /// <param name="bindingContext">Контекст привязки модели.</param>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var value = valueProviderResult.FirstValue;

            if (DateOnly.TryParseExact(value, _formatDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
                throw new FormatException("Неправильный формат даты!. Используйте следующий формат: 'yyyy-mm-dd'.");
            }
            
            return Task.CompletedTask;
        }
    }
}