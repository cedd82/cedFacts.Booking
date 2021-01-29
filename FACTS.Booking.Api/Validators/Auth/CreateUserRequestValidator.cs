using System.Text.RegularExpressions;

using FACTS.GenericBooking.Api.Models.Auth;

using FluentValidation;

namespace FACTS.GenericBooking.Api.Validators.Auth
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(m => m.Password)
                .NewPassword(8, 30);
            RuleFor(m => m.Username)
                .NotEmpty()
                .WithMessage("A0351 username is required")
                .Length(3, 8)
                .WithMessage("A0352 username must be between 3 and 8 characters")
                .Matches(@"^\w+$", RegexOptions.IgnoreCase)
                .WithMessage("A0353 username must be characters in range [a-z][A-Z]");
            RuleFor(m => m.FirstName)
                .NotEmpty()
                .WithMessage("A0354 firstName is required")
                .Length(2, 15)
                .WithMessage("A0355 firstName must be between 3 and 8 characters")
                .Matches(@"^\w+$", RegexOptions.IgnoreCase)
                .WithMessage("A0356 firstName must be characters in range [a-z][A-Z]");
            RuleFor(m => m.LastName)
                .NotEmpty()
                .WithMessage("A0357 lastName is required")
                .Length(2, 15)
                .WithMessage("A0358 lastName must be between 3 and 8 characters")
                .Matches(@"^\w+$", RegexOptions.IgnoreCase)
                .WithMessage("A0359 lastName must be characters in range [a-z][A-Z]");
            RuleFor(m => m.EmailAddress)
                .EmailAddress()
                .WithMessage("A0360 invalid emailAddress");
            RuleFor(m => m.MobileNumber)
                .NotEmpty()
                .WithMessage("A0361 mobileNumber is required")
                .Length(10)
                .WithMessage("A0362 mobileNumber must be 10 characters")
                .Matches(@"^\d{10}$")
                .WithMessage("A0363 mobileNumber must be a number 10 digits long");
        }
    }
}
