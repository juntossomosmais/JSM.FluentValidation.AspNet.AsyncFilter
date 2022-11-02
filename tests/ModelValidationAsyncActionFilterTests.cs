using FluentAssertions;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Extensions;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Models;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Startups;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests
{
    public class ModelValidationActionFilterTests : WebAppFixture<StartupWithDefaultOptions>
    {
        private const string ControllerWithApiAttributeEndpoint = "WithApiAttribute";
        private const string ControllerWithoutApiAttributeEndpoint = "WithoutApiAttribute";

        private HttpClient Client { get; }

        public ModelValidationActionFilterTests() => Client = CreateClient();

        [Theory(DisplayName = "Should return OK when payload is valid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_PayloadIsValid_ReturnOk(string controller)
        {
            // Arrange
            var payload = new TestPayload { Text = "Test" };

            // Act
            var response = await Client.PostAsJsonAsync($"{controller}/test-validator", payload);

            // Assert
            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Should return bad request when payload is invalid on a GET endpoint")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_GetEndpointWithInvalidPayload_ReturnBadRequest(string controller)
        {
            // Arrange

            // Act
            var response = await Client.GetAsync($"{controller}/test-validator?text=");

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            responseDetails.Type.Should().Be(RuleTypeConst.TypeDefault);
            responseDetails.Error.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Text", new[] { "Text can't be null" } }
            });
        }

        [Theory(DisplayName = "Should return bad request when payload is invalid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_PayloadIsInvalid_ReturnBadRequest(string controller)
        {
            // Arrange
            var payload = new TestPayload { Text = "" };

            // Act
            var response = await Client.PostAsJsonAsync($"{controller}/test-validator", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            responseDetails.Type.Should().Be(RuleTypeConst.TypeDefault);
            responseDetails.Error.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Text", new[] { "Text can't be null" } }
            });
        }

        [Theory(DisplayName = "Should return bad request when objects in collection are invalid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_ObjectsInCollectionInValid_ReturnBadRequest(string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload { Text = "" },
                new TestPayload { Text = "" }
            };

            // Act
            var response = await Client.PostAsJsonAsync($"{controller}/test-validator-collection", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            responseDetails.Type.Should().Be(RuleTypeConst.TypeDefault);
            responseDetails.Error.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Text", new[] { "Text can't be null", "Text can't be null" } }
            });
        }

        [Theory(DisplayName = "Should return ok when collection and objects are valid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_ObjectsInCollectionAreValid_ReturnOk(string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload { Text = "Test" },
                new TestPayload { Text = "Test" }
            };
            
            // Act
            var response = await Client.PostAsJsonAsync($"{controller}/test-validator-collection", payload);

            // Assert
            response
                .Should()
                .Be200Ok();
        }

        [Theory(DisplayName = "Should return bad request when collection is valid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_CollectionIsInvalid_ReturnBadRequest(string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload { Text = "Test" },
                new TestPayload { Text = "Test" },
                new TestPayload { Text = "Test" }
            };

            // Act
            var response = await Client.PostAsJsonAsync($"{controller}/test-validator-collection", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            responseDetails.Type.Should().Be(RuleTypeConst.TypeDefault);
            responseDetails.Error.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Count", new[] { "Should be less than 3!" } }
            });
        }

        [Theory(DisplayName = "Should not validate objects when collection is invalid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_CollectionIsInvalid_ShouldNotValidateObjects(string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload { Text = "" },
                new TestPayload { Text = "" },
                new TestPayload { Text = "" }
            };

            // Act
            var response = await Client.PostAsJsonAsync($"{controller}/test-validator-collection", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            responseDetails.Type.Should().Be(RuleTypeConst.TypeDefault);
            responseDetails.Error.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Count", new[] { "Should be less than 3!" } }
            });
        }

        [Theory(DisplayName = "Should return OK when the request class does not have a validator")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_ClassWithoutValidator_ReturnOk(string controller)
        {
            // Arrange
            var payload = new TestPayloadWithoutValidation { Text = "" };

            // Act
            var response = await Client.PostAsJsonAsync($"{controller}/without-validation", payload);

            // Assert
            response.Should().Be200Ok();
        }
    }
}
