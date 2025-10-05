using FluentValidation;

namespace BanksAccount.CQRS.Accounts.Commands.Create;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x  => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("UserId must be greater than zero")
            .WithErrorCode("ERROR_USER_ID");
        
        RuleFor(x => x.AccountName)
            .NotEmpty()
            .Length(1, 10)
            .WithMessage("AccountName must be between 1 and 10 characters");
        
        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage("Account type must be in enum");
    }
}