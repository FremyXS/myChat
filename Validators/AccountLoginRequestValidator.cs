using FluentValidation;
using Pract.Requests;

namespace Pract.Validators
{
    public class AccountLoginRequestValidator : AbstractValidator<AccountLoginRequest>
    {
        public AccountLoginRequestValidator()
        {
            RuleFor(x => x.Login).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
