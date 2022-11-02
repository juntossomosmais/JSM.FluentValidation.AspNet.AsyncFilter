using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    /// <summary>
    /// Extension to override error code with prefix and type.
    /// </summary>
    public static class RuleTypeBuilderExtensions
    {
        /// <summary>
        /// Overrides the error code associated with this type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="rule">The current rule</param>
        /// <param name="type">The type used to override error code</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> WithRuleType<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string type)
        {
            string propertyName = "";

            var options = rule.Configure(x => propertyName = x.GetDisplayName(null));

            options.WithErrorCode($"{RuleTypeConst.Prefix}.{type}");

            return options;
        }

        internal static string GetLastRuleType(this ModelStateDictionary modelState) =>

            GetRuleTypeInModalState(modelState) ?? RuleTypeConst.TypeDefault;

        /// <summary>
        /// Get Rule Type registered in format {Prefix}.{Type}.{Key}
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private static string GetRuleTypeInModalState(ModelStateDictionary modelState)
        {
            var lastErrorType = modelState.LastOrDefault(error => error.Key.Contains(RuleTypeConst.Prefix));
            if (string.IsNullOrEmpty(lastErrorType.Key))
                return null;
            else
            {
                int indexType = 1;
                var type = lastErrorType.Key.Split('.')[indexType];
                return type;
            }
        }
            
    }
}
