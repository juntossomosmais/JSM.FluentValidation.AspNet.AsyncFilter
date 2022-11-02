using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    internal class ApiBehaviorOptionsSetup : IConfigureOptions<ApiBehaviorOptions>
    {
        private readonly IConfigureOptions<ApiBehaviorOptions> _options;
        public ApiBehaviorOptionsSetup(IConfigureOptions<ApiBehaviorOptions> configureOptions)
        {
            _options = configureOptions;
        }

        public void Configure(ApiBehaviorOptions options)
        {
            _options.Configure(options);
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                return ConfigureValidationResponseFormat(problemDetailsFactory, context);
            };
        }

        private static IActionResult ConfigureValidationResponseFormat(ProblemDetailsFactory problemDetailsFactory, ActionContext context)
        {
            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
            if (problemDetails.Status == (int)HttpStatusCode.BadRequest)
            {
                var response = new ErrorResponse
                {
                    Error = new Dictionary<string, string[]>(),
                    Type = context.ModelState.GetLastRuleType()
                };

                foreach (var (key, value) in context.ModelState.Where(x => x.Value.Errors.Any()))
                {
                    var errorKey = key.Contains(RuleTypeConst.Prefix) ? RuleTypeConst.KeyErrorDefault : key;
                    var errorMessage = value.Errors.Select(e => e.ErrorMessage).ToArray();
                    response.Error.Add(errorKey, errorMessage);
                }

                return new BadRequestObjectResult(response);
            }

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status,
                ContentTypes =
                {
                    "application/problem+json",
                    "application/problem+xml",
                }
            };
        }
    }
}
