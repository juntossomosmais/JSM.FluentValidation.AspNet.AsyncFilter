using FluentValidation;
using FluentValidation.AspNetCore;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Startups
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