namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal class ForbiddenResponse : IErrorResponse
    {
        public ForbiddenResponse(string message, string traceparent)
        {
            TraceId = traceparent;
            Error = new ErrorMessage { Msg = message };
        }

        public string Type => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";

        public string Title => ErrorCode.Forbidden;

        public int Status => 403;

        public string TraceId { get; set; }

        public ErrorMessage Error { get; set; }
    }
}
