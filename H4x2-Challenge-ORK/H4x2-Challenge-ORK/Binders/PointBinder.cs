using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Buffers.Text;
using H4x2_Challenge_ORK.Helpers;
using H4x2_TinySDK.Ed25519;

namespace H4x2_Challenge_ORK.Binders
{
    public class PointBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None || string.IsNullOrWhiteSpace(valueProviderResult.FirstValue))
            {
                return Task.CompletedTask;
            }

            if (!EncodedPointHelper.TryFromBase64String(valueProviderResult.FirstValue, out byte[] buffer))
            {
                bindingContext.ModelState.TryAddModelError(modelName, $"Is not a valid base64 string.");
                return Task.CompletedTask;
            }

            var model = Point.FromBytes(buffer);
            if (!model.IsValid())
            {
                bindingContext.ModelState.TryAddModelError(modelName, $"Is not a valid point.");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
