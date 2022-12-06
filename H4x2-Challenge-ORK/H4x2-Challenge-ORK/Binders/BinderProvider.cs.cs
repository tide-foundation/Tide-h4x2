using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using H4x2_TinySDK.Ed25519;

namespace H4x2_Challenge_ORK.Binders
{
    public class BinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(Point))
                return new BinderTypeModelBinder(typeof(PointBinder));

            return null;
        }
    }
}
