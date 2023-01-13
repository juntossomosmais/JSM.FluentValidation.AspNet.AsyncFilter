using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal class NotFoundResponse : TraceableProblemDetails
    {
        public NotFoundResponse(string message, string traceparent)
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            Title = ErrorCode.NotFound;
            Status = 404;
            Detail = message;
            TraceId = traceparent;
        }
    }
}
