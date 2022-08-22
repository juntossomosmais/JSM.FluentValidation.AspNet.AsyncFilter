using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support
{
    public class WebAppFixture<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
        }

        public HttpClient CreateClientWithServices(Action<IServiceCollection> configurator)
        {
            return WithWebHostBuilder(builder => builder.ConfigureServices(configurator)).CreateClient();
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = true)
                .UseStartup<TStartup>();
        }
    }
}