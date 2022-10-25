using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JSM.FluentValidation.AspNet.AsyncFilter
{
    /// <summary>
    /// Extension to override error code with prefix and type.
    /// </summary>
    public static class RuleTypeBuilderExtensions
    {
        /// <summary>
        /// Overrides the error code associated with this rule with prefix and type
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

            if (!string.IsNullOrEmpty(propertyName))
                options.WithName(propertyName);

            options.WithErrorCode($"{RuleTypeConst.Prefix}.{type}");

            return options;
        }
    }
}
