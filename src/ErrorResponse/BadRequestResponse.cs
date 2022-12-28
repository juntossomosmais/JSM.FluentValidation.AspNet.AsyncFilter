namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    public class BadRequestResponse : IErrorResponse
    {
        public BadRequestResponse(string message, string traceparent)
        {
            TraceId = traceparent;
            Error = new ErrorMessage { Msg = message };
        }

        public string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        public string Title => ErrorCode.BadRequest;

        public int Status => 400;

        public string TraceId { get; set; }

        public ErrorMessage Error { get; set; }
    }
}
