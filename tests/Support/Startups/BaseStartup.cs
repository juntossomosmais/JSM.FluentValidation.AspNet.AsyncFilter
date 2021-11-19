using FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Startups
{
    public abstract class BaseStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<IValidatorFactory>(s => new ServiceProviderValidatorFactory(s))
                .AddScoped<IValidator<TestPayload>, TestPayloadValidator>()
                .AddScoped<IValidator<List<TestPayload>>, TestPayloadCollectionValidator>();
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