using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationExample.Models;

namespace ModelValidationExample.CustomModelBinders
{
    public class PersonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Person person = new();

            if(bindingContext.ValueProvider.GetValue("firstname").Length > 0)
            {
                person.Name = bindingContext.ValueProvider.GetValue("firstname").FirstValue;
            }
            if(bindingContext.ValueProvider.GetValue("lastname").Length > 0)
            {
                person.Name += " " + bindingContext.ValueProvider.GetValue("lastname").FirstValue;
            }

            bindingContext.Result = ModelBindingResult.Success(person);

            return Task.CompletedTask;
        }
    }
}
