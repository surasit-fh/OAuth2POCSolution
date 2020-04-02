using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using OAuth2POC.Model.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2POC.Model.ValidationAttributeHandle
{
    public class FromHeaderBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            BaseHeader headerResponse = new BaseHeader();

            var headercontext = bindingContext.ActionContext.HttpContext.Request.Headers;

            PropertyInfo[] propertyInfos = typeof(BaseHeader).GetProperties();

            foreach (var key in propertyInfos)
            {
                StringValues values;

                if (headercontext.TryGetValue(key.Name.Replace('_', '-'), out values))
                {
                    headerResponse.GetType().GetProperty(key.Name).SetValue(headerResponse, values.FirstOrDefault());
                }
            }

            bindingContext.Result = ModelBindingResult.Success(headerResponse);

            return Task.CompletedTask;
        }
    }
}