using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal static class ErrorResponseFactory
    {
        public static IErrorResponse CreateErrorResponse(ModelStateDictionary modelState,
            string traceparent)
        {
            if (modelState[ErrorCode.Forbidden] is not null)
                return new ForbiddenResponse(
                    modelState[ErrorCode.Forbidden]?.Errors.FirstOrDefault()?.ErrorMessage ??
                    String.Empty, traceparent);

            return new NotFoundResponse(
                modelState[ErrorCode.NotFound]?.Errors.FirstOrDefault()?.ErrorMessage ??
                String.Empty,
                traceparent);
        }

        public static HttpStatusCode GetResponseStatusCode(ModelStateDictionary modelState)
        {
            if (modelState[ErrorCode.Forbidden] is not null)
                return HttpStatusCode.Forbidden;

            return modelState[ErrorCode.NotFound] is not null
                ? HttpStatusCode.NotFound
                : HttpStatusCode.BadRequest;
        }
    }
}
