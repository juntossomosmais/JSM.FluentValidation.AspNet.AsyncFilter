using System.Collections.Generic;

namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    /// <summary>
    /// Defines what HTTP status code should be returned. Use it with the `WithErrorCode` extension method.
    /// </summary>
    public static class ErrorCode
    {
        /// <summary>
        /// 401 HTTP status code as per RFC 2616
        /// </summary>
        public const string Unauthorized = "UNAUTHORIZED_ERROR";
        
        /// <summary>
        /// 403 HTTP status code as per RFC 2616
        /// </summary>
        public const string Forbidden = "FORBIDDEN_ERROR";
        
        /// <summary>
        /// 404 HTTP status code as per RFC 2616
        /// </summary>
        public const string NotFound = "NOT_FOUND_ERROR";

        internal static readonly HashSet<string> AvailableCodes = new HashSet<string>()
            {Unauthorized, Forbidden, NotFound};
    }
}
