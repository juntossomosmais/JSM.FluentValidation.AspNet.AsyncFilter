namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal class NotFoundResponse : IErrorResponse
    {
        public NotFoundResponse(string message, string traceparent)
        {
            TraceId = traceparent;
            Error = new ErrorMessage { Msg = message };
        }

        public string Type => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";

        public string Title => ErrorCode.NotFound;

        public int Status => 404;

        public string TraceId { get; set; }

        public ErrorMessage Error { get; set; }
    }
}
