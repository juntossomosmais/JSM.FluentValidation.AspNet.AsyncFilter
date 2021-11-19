using System.Collections.Generic;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Models
{
    public class TestPayload
    {
        public string? Text { get; set; }
    }

    public class TestPayloadValidator : AbstractValidator<TestPayload>
    {
        public TestPayloadValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .WithMessage("Text can't be null");
        }
    }

    public class TestPayloadCollectionValidator : AbstractValidator<List<TestPayload>>
    {
        public TestPayloadCollectionValidator()
        {
            RuleFor(x => x.Count)
                .LessThan(3)
                .WithMessage("Should be less than 3!")
                .WithName("Items");
        }
    }
}