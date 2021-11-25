using Microsoft.Extensions.DependencyInjection;
using System;

namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    /// <summary>
    ///
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IMvcBuilder AddModelValidationAsyncActionFilter(this IMvcBuilder builder,
            Action<ModelValidationOptions> optionsAction)
        {
            builder.AddMvcOptions(mvcOptions => { mvcOptions.Filters.Add<ModelValidationAsyncActionFilter>(); });

            builder.Services.Configure(optionsAction);

            return builder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddModelValidationAsyncActionFilter(this IMvcBuilder builder)
        {
            return builder
                .AddModelValidationAsyncActionFilter(_ => { });
        }
    }
}