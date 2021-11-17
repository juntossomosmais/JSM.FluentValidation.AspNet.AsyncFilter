using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace FluentValidation.AspNet.AsyncValidationFilter
{
    /// <summary>
    /// Validates values before the controller's action is invoked (before the route is executed).
    /// Integrates with FluentValidation and allows using asynchronous rules unlike the automatic
    /// ASP.NET's validation pipeline which runs synchronously.
    /// </summary>
    public class ModelValidationAsyncActionFilter : IAsyncActionFilter
    {
        private readonly ApiBehaviorOptions _apiBehaviorOptions;
        private readonly IValidatorFactory _validatorFactory;
        private readonly ILogger<ModelValidationAsyncActionFilter> _logger;

        public ModelValidationAsyncActionFilter(
            IOptions<ApiBehaviorOptions> apiBehaviorOptions,
            IValidatorFactory validatorFactory,
            ILogger<ModelValidationAsyncActionFilter> logger)
        {
            _apiBehaviorOptions = apiBehaviorOptions.Value;
            _validatorFactory = validatorFactory;
            _logger = logger;
        }

        /// <summary>
        /// Validates values before the controller's action is invoked (before the route is executed).
        /// </summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var (_, value) in context.ActionArguments)
            {
                if (value == null)
                    continue;

                await ValidateAsync(value, context.ModelState);

                // if an error is found or the type it not enumerable, short circuit the loop
                if (!context.ModelState.IsValid || !TypeIsEnumerable(value.GetType())) continue;

                await ValidateEnumerableObjectsAsync(value, context.ModelState);
            }

            if (!context.ModelState.IsValid)
            {
                _logger.LogDebug("The request has model state errors, returning an error response.");
                context.Result = _apiBehaviorOptions.InvalidModelStateResponseFactory(context);
                return;
            }

            await next();
        }

        private async Task ValidateEnumerableObjectsAsync(object value, ModelStateDictionary modelState)
        {
            var underlyingType = value.GetType().GenericTypeArguments[0];
            var validator = _validatorFactory.GetValidator(underlyingType);

            if (validator == null)
                return;

            foreach (var item in (IEnumerable)value)
            {
                var context = new ValidationContext<object>(item);
                var result = await validator.ValidateAsync(context);

                result.AddToModelState(modelState, string.Empty);
            }
        }

        private async Task ValidateAsync(object value, ModelStateDictionary modelState)
        {
            var validator = _validatorFactory.GetValidator(value.GetType());

            if (validator == null)
                return;

            var context = new ValidationContext<object>(value);
            var result = await validator.ValidateAsync(context);
            result.AddToModelState(modelState, string.Empty);
        }

        private static bool TypeIsEnumerable(Type type) =>
            type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type);
    }
}

