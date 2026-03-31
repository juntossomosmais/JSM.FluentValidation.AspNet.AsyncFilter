using FluentValidation;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Startups
{
    public abstract class BaseStartup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddValidatorsFromAssemblyContaining<TestPayloadValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
                endpoints.MapDefaultControllerRoute());
        }
    }
}