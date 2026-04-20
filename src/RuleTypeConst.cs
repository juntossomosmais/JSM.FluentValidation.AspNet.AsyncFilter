namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    /// <summary>
    /// Const used do configure method WithRuleType.
    /// </summary>
    public static class RuleTypeConst
    {
        /// <summary>
        /// Prefix used to override property name.
        /// </summary>
        public const string Prefix = "_RuleType";
        
        /// <summary>
        /// Type default used in response bad request
        /// </summary>
        public const string TypeDefault = "VALIDATION_ERRORS";

        /// <summary>
        /// Key error default
        /// </summary>
        public const string KeyErrorDefault = "msg";

    }
}