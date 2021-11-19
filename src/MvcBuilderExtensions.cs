using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentValidation.AspNet.AsyncValidationFilter
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddModelValidationAsyncActionFilter(this IMvcBuilder builder,
            Action<ModelValidationOptions> optionsAction)
        {
            builder.AddMvcOptions(mvcOptions => { mvcOptions.Filters.Add<ModelValidationAsyncActionFilter>(); });

            builder.Services.Configure(optionsAction);

            return builder;
        }

        public static IMvcBuilder AddModelValidationAsyncActionFilter(this IMvcBuilder builder)
        {
            return builder
                .AddModelValidationAsyncActionFilter(_ => { });
        }
    }
}