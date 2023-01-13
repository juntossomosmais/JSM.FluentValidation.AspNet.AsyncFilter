using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using JSM.FluentValidation.AspNet.AsyncFilter.ErrorResponse;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Models
{
    public class TestUser
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class TestUserValidator : AbstractValidator<TestUser>
    {
        public TestUserValidator()
        {
            RuleFor(user => user)
                .Must(user => user.Id != "432")
                .WithMessage("Unauthorized user")
                .WithErrorCode(ErrorCode.Unauthorized);
            
            RuleFor(user => user)
                .Must(HasAlreadyBeenRegistered)
                .WithMessage("User not found")
                .WithErrorCode(ErrorCode.NotFound);

            RuleFor(user => user)
                .Must(user => user.Id != "321")
                .WithMessage("Insufficient rights to access this resource")
                .WithErrorCode(ErrorCode.Forbidden);
        }

        bool HasAlreadyBeenRegistered(TestUser user)
        {
            var storedUsers = new List<TestUser>()
            {
                new TestUser() {Id = "123", Name = "John Doe"},
                new TestUser() {Id = "321", Name = "User with insufficient rights"},
                new TestUser() {Id = "432", Name = "Unauthorized user"}
            };

            return storedUsers.Any(x => x.Id == user.Id);
        }
    }
}
