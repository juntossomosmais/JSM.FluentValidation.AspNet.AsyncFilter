using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests.Support
{
    public class StartupWithValidators
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<IValidatorFactory>(s => new ServiceProviderValidatorFactory(s))
                .AddScoped<IValidator<TestPayload>, TestPayloadValidator>()
                .AddScoped<IValidator<List<TestPayload>>, TestPayloadCollectionValidator>();
#if NETCOREAPP2_2
            services
                .AddMvc(options => options.Filters.Add<ModelValidationAsyncActionFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
#else
            services
                .AddControllers(options => options.Filters.Add<ModelValidationAsyncActionFilter>());
#endif

        }

        public void Configure(IApplicationBuilder app)
        {

#if NETCOREAPP2_2
            app.UseMvc();
#else
            app.UseRouting();
            app.UseEndpoints(endpoints =>
                endpoints.MapDefaultControllerRoute());
#endif
        }
    }
}