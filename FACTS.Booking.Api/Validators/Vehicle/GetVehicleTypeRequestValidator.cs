using FACTS.GenericBooking.Api.Models.Vehicle;

using FluentValidation;

namespace FACTS.GenericBooking.Api.Validators.Vehicle
{
    public class GetVehicleTypeRequestValidator : AbstractValidator<GetVehicleTypesRequest>
    {
        public GetVehicleTypeRequestValidator()
        {
            RuleFor(m => m.Model)
                .MaximumLength(20)
                .WithMessage("A0358 model has a maximum of 20 characters");

            RuleFor(m => m.Make)
                .MaximumLength(20)
                .WithMessage("A0359 make has a maximum of 20 characters");

            RuleFor(m => m.Type)
                .MaximumLength(20)
                .WithMessage("A0360 type has a maximum of 20 characters");
        }
    }
}