using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Startups
{
    public class StartupWithDefaultOptions : BaseStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services
                .AddControllers()
                .AddModelValidationAsyncActionFilter();
        }
    }
}