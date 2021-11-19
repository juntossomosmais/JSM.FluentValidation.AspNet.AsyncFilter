namespace FluentValidation.AspNet.AsyncValidationFilter
{
    public class ModelValidationOptions
    {
        /// <summary>
        /// Determines if the filter will be applied only to controllers with the
        /// ApiControllerAttribute
        /// </summary>
        public bool OnlyApiController { get; set; } = false;
    }
}