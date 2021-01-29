using System.Text.RegularExpressions;

using FACTS.GenericBooking.Api.Models.Quote;
using FACTS.GenericBooking.Common.ExtensionMethods;

using FluentValidation;

namespace FACTS.GenericBooking.Api.Validators.Quote
{
    public class GetRatesRequestValidator : AbstractValidator<GetRatesRequest>
    {
        public GetRatesRequestValidator()
        {
            RuleFor(m => m.AccountNumber)
                .NotEmpty()
                .WithMessage("A0004 accountNumber is required")
                .Length(6)
                .WithMessage("A0005 accountNumber must be 6 characters");
            RuleFor(m => m.ServiceType)
                .Matches("^(Standard|Express|PremiumEnclosed)?$", RegexOptions.IgnoreCase)
                .When(m => !string.IsNullOrEmpty(m.ServiceType))
                .WithMessage("A0008 serviceType must either 'standard','express','premiumEnclosed'");
            RuleFor(m => m.PickupType)
                .Matches("^(customer|depot)?$", RegexOptions.IgnoreCase)
                .When(m => !string.IsNullOrEmpty(m.PickupType))
                .WithMessage("A0010 pickupType must be 'customer' or 'depot'");
            RuleFor(m => m.PickupSuburb)
                .NotEmpty()
                .WithMessage("A0011 pickupSuburb is required")
                .Length(3, 20)
                .WithMessage("A0012 pickupSuburb must be between 3 and 20 characters");
            RuleFor(m => m.PickupPostcode)
                .NotEmpty()
                .WithMessage("A0013 pickupPostCode is required")
                .Matches(@"^\d{4}$")
                .WithMessage("A0014 pickupPostCode must be a number 4 digits long");
            RuleFor(m => m.PickupState)
                .NotEmpty()
                .WithMessage("A0015 pickupState is required")
                .Length(2, 3)
                .WithMessage("A0016 pickupState must be either 2 or 3 characters")
                .Must(x => x.IsValidAustralianState())
                .WithMessage("A0017 pickupState is invalid. It must be either NSW, QLD, SA, TAS, VIC, WA, ACT or NT");
            RuleFor(m => m.DeliveryType)
                .Matches("^(customer|depot)?$", RegexOptions.IgnoreCase)
                .When(m => !string.IsNullOrEmpty(m.DeliveryType))
                .WithMessage("A0019 deliveryType must be 'customer' or 'depot'");
            RuleFor(m => m.DeliverySuburb)
                .NotEmpty()
                .WithMessage("A0020 deliverySuburb is required")
                .Length(3, 20)
                .WithMessage("A0021 deliverySuburb must be between 3 and 20 characters");
            RuleFor(m => m.DeliveryPostcode)
                .NotEmpty()
                .WithMessage("A0022 deliveryPostCode is required")
                .Matches(@"^\d{4}$")
                .WithMessage("A0023 deliveryPostCode must be a number 4 digits long");
            RuleFor(m => m.DeliveryState)
                .NotEmpty()
                .WithMessage("A0024 deliveryState is required")
                .Length(2, 3)
                .WithMessage("A0025 deliveryState must be either 2 or 3 characters")
                .Must(x => x.IsValidAustralianState())
                .WithMessage("A0026 deliveryState is invalid. It must be either NSW, QLD, SA, TAS, VIC, WA, ACT or NT");
            RuleFor(m => m.IsDriveable)
                .NotEmpty()
                .WithMessage("A0027 isDriveable is required")
                .Must(m => m == true)
                .WithMessage("A0007 vehicle isDriveable set to false. Contact CEVA directly for a rate.");
            RuleFor(m => m.IsModified)
                .NotEmpty()
                .WithMessage("A0028 isModified is required")
                .Must(m => m == false)
                .WithMessage("A0028 vehicle isModified set to true. Contact CEVA directly for a rate.");
            RuleFor(m => m.IsDamaged)
                .NotEmpty()
                .WithMessage("A0029 isDamaged is required")
                .Must(m => m == false)
                .WithMessage("A0029 vehicle isDamaged set to true. Contact CEVA directly for a rate.");
            RuleFor(m => m.VehicleMake)
                .NotEmpty()
                .WithMessage("A0030 vehicleMake is required")
                .Length(1, 20)
                .WithMessage("A0031 vehicleMake must be between 1 and 20 characters");
            RuleFor(m => m.VehicleModel)
                .NotEmpty()
                .WithMessage("A0032 vehicleModel is required")
                .Length(1, 20)
                .WithMessage("A0033 vehicleModel must be between 1 and 20 characters");
            RuleFor(m => m.VehicleType)
                .NotEmpty()
                .WithMessage("A0034 vehicleType is required")
                .Length(1, 20)
                .WithMessage("A0035 vehicleType must be between 1 and 20 characters");
            RuleFor(m => m.VehicleValue)
                .NotNull()
                .WithMessage("A0036 vehicleValue is required")
                .GreaterThan(100)
                .WithMessage("A0037 vehicleValue must be greater than 100 and less than 100,000")
                .LessThan(100000)
                .WithMessage("A0037 the vehicleValue is greater than 100,000; contact support for a rate.");
        }
    }
}