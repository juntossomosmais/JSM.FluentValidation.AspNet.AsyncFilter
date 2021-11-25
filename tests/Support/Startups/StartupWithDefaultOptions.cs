using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Startups
{
    public class StartupWithDefaultOptions : BaseStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
#if NETCOREAPP2_2
            services
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
#else
            services
                .AddControllers()
#endif
                .AddModelValidationAsyncActionFilter();
        }
    }
}