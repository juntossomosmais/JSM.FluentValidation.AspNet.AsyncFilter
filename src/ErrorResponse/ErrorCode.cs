using System.Collections.Generic;

namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    public static class ErrorCode
    {
        public const string BadRequest = "VALIDATION_ERROR";
        public const string NotFound = "NOT_FOUND_ERROR";
        public const string Forbidden = "FORBIDDEN_ERROR";

        internal static readonly HashSet<string> AvailableCodes = new HashSet<string>()
            {BadRequest, NotFound, Forbidden};
    }
}
