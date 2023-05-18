using FluentValidation;
using Pract.Requests;

namespace Pract.Validators
{
    public class AccountCreateRequestValidator: AbstractValidator<AccountCreateRequest>
    {
        public AccountCreateRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(x => x.Login).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
