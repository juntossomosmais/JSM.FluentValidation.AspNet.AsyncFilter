namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal interface IErrorResponse
    {
        public string Type { get; }

        public string Title { get; }

        public int Status { get; }

        public string TraceId { get; }

        public ErrorMessage Error { get; }
    }
}
