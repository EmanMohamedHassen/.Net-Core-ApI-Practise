using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //our binding works only on enumerable types
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            //Get the inputted value through the vale provider
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            //If that value is null or whitespace , we return null
            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);// to return a bad request ;
                return Task.CompletedTask;
            }

            //the value isn't null or whitespace 
            //and the type of the model is enumerable
            //Get the enumerable's type , and a converter
            var elementType = bindingContext.ModelType.GetType().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            //convert each item in the value list to the enumerable type 
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => converter.ConvertFromString(x.Trim())).ToArray();

            // create an array of that type, and set itas the model value 
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;
            
            //return a successful result , passing the model 
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
