namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal class ForbiddenResponse : TraceableProblemDetails
    {
        public ForbiddenResponse(string message, string traceparent)
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
            Title = ErrorCode.Forbidden;
            Status = 403;
            Detail = message;
            TraceId = traceparent;
        }
    }
}
