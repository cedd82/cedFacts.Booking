using FACTS.GenericBooking.Api.Models.Auth;
using FACTS.GenericBooking.Common.Models.Domain.Messages;

using FluentValidation;

namespace FACTS.GenericBooking.Api.Validators.Auth
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(m => m.OldPassword)
                .NotEmpty()
                .WithMessage(ErrorMessages.ErrUserMissingPassword.ToString());
            RuleFor(m => m.NewPassword)
                .NewPassword(8, 30);
        }
    }
}