using FluentValidation;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Models
{
    public class TestPayloadWithRuleType
    {
        public string Text { get; set; }
    }

    public class TestPayloadWithRuleTypeValidator : AbstractValidator<TestPayloadWithRuleType>
    {
        public TestPayloadWithRuleTypeValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Text can't be null")
                .WithRuleType("SOME_RULE");
        }
    }
}