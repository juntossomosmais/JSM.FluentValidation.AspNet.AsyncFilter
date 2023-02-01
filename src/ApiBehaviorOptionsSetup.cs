using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

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
                ConfigureValidationResponseFormat(context);
        }

        private static IActionResult ConfigureValidationResponseFormat(ActionContext context)
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
    }
}
