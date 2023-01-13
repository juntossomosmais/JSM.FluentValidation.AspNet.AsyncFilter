using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse;
using Microsoft.AspNetCore.Http;

namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    /// <summary>
    /// Validates values before the controller's action is invoked (before the route is executed).
    /// Integrates with FluentValidation and allows using asynchronous rules unlike the automatic
    /// ASP.NET's validation pipeline which runs synchronously.
    /// </summary>
    public class ModelValidationAsyncActionFilter : IAsyncActionFilter
    {
        private readonly ApiBehaviorOptions _apiBehaviorOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ModelValidationAsyncActionFilter> _logger;
        private readonly ModelValidationOptions _modelValidationOptions;

        /// <summary>
        /// Create a new model validation async action filter
        /// </summary>
        /// <param name="apiBehaviorOptions"></param>
        /// <param name="modelValidationOptions"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public ModelValidationAsyncActionFilter(
            IOptions<ApiBehaviorOptions> apiBehaviorOptions,
            IOptions<ModelValidationOptions> modelValidationOptions,
            IServiceProvider serviceProvider,
            ILogger<ModelValidationAsyncActionFilter> logger)
        {
            _apiBehaviorOptions = apiBehaviorOptions.Value;
            _modelValidationOptions = modelValidationOptions.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Validates values before the controller's action is invoked (before the route is executed).
        /// </summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (ShouldIgnoreFilter(context))
            {
                await next();
                return;
            }

            await ValidateActionArguments(context);

            if (!context.ModelState.IsValid)
            {
                _logger.LogDebug(
                    "The request has model state errors, returning an error response");
                var responseStatusCode =
                    ErrorResponseFactory.GetResponseStatusCode(context.ModelState);

                // BadRequest responses will return the default response structure, only different
                // status codes will be customized
                if (responseStatusCode == HttpStatusCode.BadRequest)
                {
                    context.Result = _apiBehaviorOptions.InvalidModelStateResponseFactory(context);
                    return;
                }

                var errorResponse =
                    ErrorResponseFactory.CreateErrorResponse(context.ModelState,
                        Activity.Current?.Id ?? context.HttpContext.TraceIdentifier);

                context.HttpContext.Response.StatusCode = (int) responseStatusCode;
                context.HttpContext.Response.ContentType = "application/json";

                var responseBody = JsonSerializer.Serialize(errorResponse);
                await context.HttpContext.Response.WriteAsync(responseBody);

                return;
            }

            await next();
        }

        private async Task ValidateActionArguments(ActionExecutingContext context)
        {
            foreach (var (_, value) in context.ActionArguments)
            {
                if (value is null)
                    continue;

                await ValidateAsync(value, context.ModelState);

                // if an error is found or the type it not enumerable, short circuit the loop
                if (!context.ModelState.IsValid || !TypeIsEnumerable(value.GetType())) continue;

                await ValidateEnumerableObjectsAsync(value, context.ModelState);
            }
        }

        private bool ShouldIgnoreFilter(ActionExecutingContext context)
        {
            if (!_modelValidationOptions.OnlyApiController)
                return false;

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return false;

            var hasApiControllerAttribute = controllerActionDescriptor.ControllerTypeInfo
                .GetCustomAttributes(inherit: true)
                .Any(x => x.GetType() == typeof(ApiControllerAttribute));
            return !hasApiControllerAttribute;
        }

        private async Task ValidateEnumerableObjectsAsync(object value,
            ModelStateDictionary modelState)
        {
            var underlyingType = value.GetType().GenericTypeArguments[0];
            var validator = GetValidator(underlyingType);

            if (validator == null)
                return;

            foreach (var item in (IEnumerable) value)
            {
                if (item is null)
                    continue;
                var context = new ValidationContext<object>(item);
                var result = await validator.ValidateAsync(context);

                var errorCode = result.Errors?.FirstOrDefault()?.ErrorCode;

                result.AddToModelState(modelState,
                    ErrorCode.AvailableCodes.Contains(errorCode) ? errorCode : string.Empty);
            }
        }

        private async Task ValidateAsync(object value, ModelStateDictionary modelState)
        {
            var validator = GetValidator(value.GetType());

            if (validator == null)
                return;

            var context = new ValidationContext<object>(value);
            var result = await validator.ValidateAsync(context);

            var errorCode = result.Errors?.FirstOrDefault()?.ErrorCode;

            result.AddToModelState(modelState,
                ErrorCode.AvailableCodes.Contains(errorCode) ? errorCode : string.Empty);
        }

        private IValidator GetValidator(Type targetType)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(targetType);
            var validator = (IValidator) _serviceProvider.GetService(validatorType);
            return validator;
        }

        private static bool TypeIsEnumerable(Type type) =>
            type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type);
    }
}
