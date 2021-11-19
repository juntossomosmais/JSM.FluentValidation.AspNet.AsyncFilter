namespace FluentValidation.AspNet.AsyncValidationFilter
{
    /// <summary>
    /// Options used to configure behavior for the <see cref="ModelValidationAsyncActionFilter"/>.
    /// </summary>
    public class ModelValidationOptions
    {
        /// <summary>
        /// Gets or sets a value that determines if the filter will be applied only to controllers with the
        /// ApiControllerAttribute. Others controllers will be ignored.
        /// </summary>
        public bool OnlyApiController { get; set; } = false;
    }
}