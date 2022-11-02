using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

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
            builder.Services.AddWrapper<IConfigureOptions<ApiBehaviorOptions>>((serviceProvider, t) => new ApiBehaviorOptionsSetup(t));
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

        private static IServiceCollection AddWrapper<T>(this IServiceCollection services, Func<IServiceProvider, T, T> factory)
           where T : class
        {
            var coreResolver = services.Single(x => x.ServiceType == typeof(T));
            var coreImplementationType = coreResolver.ImplementationType;
            services.Add(new ServiceDescriptor(coreImplementationType, coreImplementationType, coreResolver.Lifetime));
            services.Add(new ServiceDescriptor(typeof(T), serviceProvider => factory(serviceProvider, (T)serviceProvider.GetRequiredService(coreImplementationType)), coreResolver.Lifetime));
            return services;
        }
    }
}