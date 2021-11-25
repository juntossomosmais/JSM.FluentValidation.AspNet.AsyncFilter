using Microsoft.Extensions.DependencyInjection;
using System;

namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    /// <summary>
    /// Extensions for configuring MVC using an IMvcBuilder.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Add the ModelValidationAsyncActionFilter as a MVC Filter
        /// </summary>
        /// <param name="builder">The IMvcBuilder</param>
        /// <param name="optionsAction">A ModelValidationOptions options action</param>
        /// <returns>The IMvcBuilder</returns>
        public static IMvcBuilder AddModelValidationAsyncActionFilter(this IMvcBuilder builder,
            Action<ModelValidationOptions> optionsAction)
        {
            builder.AddMvcOptions(mvcOptions => { mvcOptions.Filters.Add<ModelValidationAsyncActionFilter>(); });

            builder.Services.Configure(optionsAction);

            return builder;
        }

        /// <summary>
        /// Add the ModelValidationAsyncActionFilter as a MVC Filter
        /// </summary>
        /// <param name="builder">The IMvcBuilder</param>
        /// <returns>The IMvcBuilder</returns>
        public static IMvcBuilder AddModelValidationAsyncActionFilter(this IMvcBuilder builder)
        {
            return builder
                .AddModelValidationAsyncActionFilter(_ => { });
        }
    }
}