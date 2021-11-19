using FluentAssertions;
using FluentValidation.AspNet.AsyncValidationFilter.Tests.Support;
using FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Extensions;
using FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Models;
using FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Startups;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests
{
    public class ModelValidationActionFilterCustomOptionsTests: WebAppFixture<StartupWithCustomOptions>
    {
        private const string ControllerWithApiAttributeEndpoint = "WithApiAttribute";
        private const string ControllerWithoutApiAttributeEndpoint = "WithoutApiAttribute";
        private HttpClient Client { get; }

        public ModelValidationActionFilterCustomOptionsTests() => Client = CreateClient();

        [Fact(DisplayName = "Should return OK (ignore validation) when controller does not have [ApiController]")]
        public async Task OnActionExecutionAsync_ControllerDoesNotHaveApiControllerAttribute_ReturnOk()
        {
            // Arrange
            var payload = new TestPayload { Text = "" };

            // Act
            var response =
                await Client.PostAsJsonAsync($"{ControllerWithoutApiAttributeEndpoint}/test-validator", payload);

            // Assert
            response.Should().Be200Ok();
        }

        [Fact(DisplayName = "Should return bad request (validation error) when controller has [ApiController]")]
        public async Task OnActionExecutionAsync_ControllerHasApiControllerAttribute_ReturnBadRequest()
        {
            // Arrange
            var payload = new TestPayload { Text = "" };

            // Act
            var response =
                await Client.PostAsJsonAsync($"{ControllerWithApiAttributeEndpoint}/test-validator", payload);

            // Assert
            response.Should().Be400BadRequest();
        }
    }
}