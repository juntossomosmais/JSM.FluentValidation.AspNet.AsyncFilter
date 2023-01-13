using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse
{
    internal abstract class TraceableProblemDetails : ProblemDetails
    {
        /// <summary>
        /// A unique identifier responsible to describe the incoming request.
        /// </summary>
        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }
}
