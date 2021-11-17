using FluentAssertions;
using FluentValidation.AspNet.AsyncValidationFilter.Tests.Support;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests
{
    public class ModelValidationActionFilterTests : WebAppFixture<StartupWithValidators>
    {
        private HttpClient Client { get; }

        public ModelValidationActionFilterTests() => Client = CreateClient();

        [Fact(DisplayName = "Should return OK when payload is valid")]
        public async Task ShouldReturnOkWhenTestPayloadIsValid()
        {
            // Arrange
            var content = new StringContent(
                JsonConvert.SerializeObject(new TestPayload { Text = "Test" }), Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync("/test-validator", content);

            // Assert
            response.Should().Be200Ok();
        }

        [Fact(DisplayName = "Should return bad request when payload is invalid")]
        public async Task ShouldReturnBadRequestWhenTestPayloadIsInvalid()
        {
            // Arrange
            var content = new StringContent(
                JsonConvert.SerializeObject(new TestPayload { Text = "" }), Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync("/test-validator", content);

            // Assert
            response.Should().Be400BadRequest();
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseJson);
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Text", new[] { "Text can't be null" } }
            });
        }

        [Fact(DisplayName = "Should return bad request when objects in collection are invalid")]
        public async Task ShouldReturnOkWhenObjectsInCollectionInValid()
        {
            // Arrange
            var content = new StringContent(
                JsonConvert.SerializeObject(
                    new[]
                    {
                        new TestPayload { Text = "" },
                        new TestPayload { Text = "" }
                    }), Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync("/test-validator-collection", content);

            // Assert
            response.Should().Be400BadRequest();
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseJson);
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Text", new[] { "Text can't be null", "Text can't be null" } }
            });
        }

        [Fact(DisplayName = "Should return ok when collection and objects are valid")]
        public async Task ShouldReturnOkWhenObjectsInCollectionAreValid()
        {
            // Arrange
            var content = new StringContent(
                JsonConvert.SerializeObject(
                    new[]
                    {
                        new TestPayload { Text = "Test" },
                        new TestPayload { Text = "Test" }
                    }), Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync("/test-validator-collection", content);

            // Assert
            response
                .Should()
                .Be200Ok();
        }

        [Fact(DisplayName = "Should return bad request when collection is valid")]
        public async Task ShouldReturnBadRequestWhenCollectionIsInvalid()
        {
            // Arrange
            var content = new StringContent(
                JsonConvert.SerializeObject(
                    new[]
                    {
                        new TestPayload { Text = "Test" },
                        new TestPayload { Text = "Test" },
                        new TestPayload { Text = "Test" }
                    }), Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync("/test-validator-collection", content);

            // Assert
            response.Should().Be400BadRequest();
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseJson);
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Count", new[] { "Should be less than 3!" } }
            });
        }

        [Fact(DisplayName = "Should not validate objects when collection is invalid")]
        public async Task ShouldNotValidateObjectsWhenCollectionIsInvalid()
        {
            // Arrange
            var content = new StringContent(
                JsonConvert.SerializeObject(
                    new[]
                    {
                        new TestPayload { Text = "" },
                        new TestPayload { Text = "" },
                        new TestPayload { Text = "" }
                    }), Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync("/test-validator-collection", content);

            // Assert
            response.Should().Be400BadRequest();
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(responseJson);
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { "Count", new[] { "Should be less than 3!" } }
            });
        }
    }
}
