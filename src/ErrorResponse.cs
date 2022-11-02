using System.Collections.Generic;

namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    /// <summary>
    /// Class serializable in response error
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Errors returned in response
        /// </summary>
        public Dictionary<string, string[]> Error { get; set; }

        /// <summary>
        /// Type returned in response
        /// </summary>
        public string Type { get; set; }
    }
}
