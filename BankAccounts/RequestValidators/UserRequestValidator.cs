using BankAccounts.Shared.Models.Requests;
using FluentValidation;

namespace BankAccounts.API.RequestValidators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Name must be at least three characters long.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email must contain '@' and be a valid email format.");

            RuleFor(x => x.UserLastName)
               .NotEmpty()
               .MinimumLength(3)
               .WithMessage("Last Name must be longer than three characters.");

            RuleFor(x => x.PhoneNumber)
               .NotEmpty()
               .Matches(@"^\+?[0-9]{10,15}$")
               .WithMessage("PhoneNumber must contain only digits and may start with '+', length from 10 to 15 characters.");

            RuleFor(x => x.DateOfBirth)
               .NotEmpty()
               .LessThan(DateTime.UtcNow)
               .WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage("Account type must be in enum");
        }
    }
}
