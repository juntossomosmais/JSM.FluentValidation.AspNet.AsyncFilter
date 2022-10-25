using FluentAssertions;
using JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support;
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
    public class ModelValidationAsyncActionFilterWithRuleTypeTests : WebAppFixture<StartupWithDefaultOptions>
    {
        private const string ControllerWithRuleTypeEndpoint = "WithRuleType";
        private HttpClient Client { get; }

        public ModelValidationAsyncActionFilterWithRuleTypeTests() => Client = CreateClient();

        [Fact(DisplayName = "Should return OK when payload is valid")]
        public async Task OnActionExecutionAsync_PayloadIsValid_ReturnOk()
        {
            // Arrange
            var payload = new TestPayloadWithRuleType { Text = "Test" };

            // Act
            var response = await Client.PostAsJsonAsync($"{ControllerWithRuleTypeEndpoint}/test-validator", payload);

            // Assert
            response.Should().Be200Ok();
        }

        [Fact(DisplayName = "Should return bad request when payload is invalid")]
        public async Task OnActionExecutionAsync_PayloadIsInvalid_ReturnBadRequest()
        {
            // Arrange
            var payload = new TestPayloadWithRuleType { Text = "" };

            // Act
            var response = await Client.PostAsJsonAsync($"{ControllerWithRuleTypeEndpoint}/test-validator", payload);

            // Assert
            response.Should().Be400BadRequest();
            var responseDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            responseDetails.Title.Should().Be("One or more validation errors occurred.");
            responseDetails.Errors.Should().BeEquivalentTo(new Dictionary<string, string[]>
            {
                { $"{RuleTypeConst.Prefix}.SOME_RULE.Text", new[] { "Text can't be null" } }
            });
        }
    }
}
