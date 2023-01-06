using FluentAssertions;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Extensions;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Models;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Startups;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
            var payload = new TestPayload {Text = "Test"};

            // Act
            var response = await HttpClientJsonExtensions.PostAsJsonAsync(Client, $"{controller}/test-validator", payload);

            // Assert
            response.Should().Be200Ok();
        }

        [Theory(DisplayName =
            "Should return bad request when payload is invalid on a GET endpoint")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_GetEndpointWithInvalidPayload_ReturnBadRequest(
            string controller)
        {
            // Arrange
            var payload = new TestPayload {Text = ""};

            // Act
            var response = await Client.GetAsync($"{controller}/test-validator?text=");

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails =
                await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                {"Text", new[] {"Text can't be null"}}
            });
        }

        [Theory(DisplayName = "Should return bad request when payload is invalid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_PayloadIsInvalid_ReturnBadRequest(
            string controller)
        {
            // Arrange
            var payload = new TestPayload {Text = ""};

            // Act
            var response = await HttpClientJsonExtensions.PostAsJsonAsync(Client, $"{controller}/test-validator", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails =
                await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                {"Text", new[] {"Text can't be null"}}
            });
        }

        [Theory(DisplayName = "Should return bad request when objects in collection are invalid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_ObjectsInCollectionInValid_ReturnBadRequest(
            string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload {Text = ""},
                new TestPayload {Text = ""}
            };

            // Act
            var response =
                await HttpClientJsonExtensions.PostAsJsonAsync(Client, $"{controller}/test-validator-collection", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails =
                await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                {"Text", new[] {"Text can't be null", "Text can't be null"}}
            });
        }

        [Theory(DisplayName = "Should return ok when collection and objects are valid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_ObjectsInCollectionAreValid_ReturnOk(
            string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload {Text = "Test"},
                new TestPayload {Text = "Test"}
            };

            // Act
            var response =
                await HttpClientJsonExtensions.PostAsJsonAsync(Client, $"{controller}/test-validator-collection", payload);

            // Assert
            response
                .Should()
                .Be200Ok();
        }

        [Theory(DisplayName = "Should return bad request when collection is valid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_CollectionIsInvalid_ReturnBadRequest(
            string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload {Text = "Test"},
                new TestPayload {Text = "Test"},
                new TestPayload {Text = "Test"}
            };

            // Act
            var response =
                await HttpClientJsonExtensions.PostAsJsonAsync(Client, $"{controller}/test-validator-collection", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails =
                await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                {"Count", new[] {"Should be less than 3!"}}
            });
        }

        [Theory(DisplayName = "Should not validate objects when collection is invalid")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_CollectionIsInvalid_ShouldNotValidateObjects(
            string controller)
        {
            // Arrange
            var payload = new[]
            {
                new TestPayload {Text = ""},
                new TestPayload {Text = ""},
                new TestPayload {Text = ""}
            };

            // Act
            var response =
                await HttpClientJsonExtensions.PostAsJsonAsync(Client, $"{controller}/test-validator-collection", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails =
                await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                {"Count", new[] {"Should be less than 3!"}}
            });
        }

        [Theory(DisplayName = "Should return OK when the request class does not have a validator")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_ClassWithoutValidator_ReturnOk(string controller)
        {
            // Arrange
            var payload = new TestPayloadWithoutValidation {Text = ""};

            // Act
            var response =
                await HttpClientJsonExtensions.PostAsJsonAsync(Client, $"{controller}/without-validation", payload);

            // Assert
            response.Should().Be200Ok();
        }

        [Theory(DisplayName = "Should return OK when the request met all requirements")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_RequestMetAllRequirements_ReturnOk(
            string controller)
        {
            // Arrange
            var payload = new TestUser {Id = "123"};

            // Act
            var response =
                await Client.GetAsync($"{controller}/user-test-validator?id={payload.Id}");

            // Assert
            response.Should().Be200Ok();
        }

        [Theory(DisplayName =
            "Should return Not Found when the request has an input that doesn't exist")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_RequestWithNonexistentInput_ReturnNotFound(
            string controller)
        {
            // Arrange
            var payload = new TestUser {Id = "333"};

            // Act
            var response =
                await Client.GetAsync($"{controller}/user-test-validator?id={payload.Id}");

            // Assert
            response.Should().Be404NotFound().And.BeAs(
                new
                {
                    type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                    title = "NOT_FOUND_ERROR",
                    status = 404,
                    traceId = "0HMH5DLVSLJDP",
                    error = new
                    {
                        msg = "User not found"
                    }
                },
                options => options.Excluding(source => source.traceId)
            );
        }

        [Theory(DisplayName =
            "Should return Forbidden when the request has an input with insufficient rights")]
        [InlineData(ControllerWithApiAttributeEndpoint)]
        [InlineData(ControllerWithoutApiAttributeEndpoint)]
        public async Task OnActionExecutionAsync_RequestWithInsufficientRightsInput_ReturnForbidden(
            string controller)
        {
            // Arrange
            var payload = new TestUser {Id = "321"};

            // Act
            var response =
                await Client.GetAsync($"{controller}/user-test-validator?id={payload.Id}");

            // Assert
            response.Should().Be403Forbidden().And.BeAs(
                new
                {
                    type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                    title = "FORBIDDEN_ERROR",
                    status = 403,
                    traceId = "",
                    error = new
                    {
                        msg = "Insufficient rights to access this resource"
                    }
                },
                options => options.Excluding(source => source.traceId)
            );
        }
    }
}
