using FACTS.GenericBooking.Api.Models.Auth;

using FluentValidation;

namespace FACTS.GenericBooking.Api.Validators.Auth
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserRequest>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(m => m.Username)
                .NotEmpty()
                .WithMessage("A0300 username is required")
                .Length(3, 8)
                .WithMessage("A0301 username must be between 3 and 8 characters");
            RuleFor(m => m.Password)
                .Password(8, 30);
        }
    }
}
