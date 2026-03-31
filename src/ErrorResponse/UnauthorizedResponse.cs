namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal class UnauthorizedResponse : TraceableProblemDetails
    {
        public UnauthorizedResponse(string message, string traceparent)
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
            Title = ErrorCode.Unauthorized;
            Status = 401;
            Detail = message;
            TraceId = traceparent;
        }
    }
}
