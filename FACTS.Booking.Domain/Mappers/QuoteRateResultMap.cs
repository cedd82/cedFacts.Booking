using System.Collections.Generic;

using FACTS.GenericBooking.Common.ExtensionMethods;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Domain.Models.Customer;
using FACTS.GenericBooking.Domain.Models.IngresDto;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Models.Vehicle;

namespace FACTS.GenericBooking.Domain.Mappers
{
    public static class QuoteRateResultMap
    {
        public static Result<GetRatesResultDto> MapQuoteRates(List<VehicleRateDto> vehicleQuotes,
                                                                GetRatesDto getRates,
                                                                VehicleDetailsDto vehicleDetails,
                                                                SuburbDto deliverLocation,
                                                                SuburbDto pickLocation,
                                                                CustomerDetailsDto customerDetails)
        {
            GetRatesResultDto getRatesMapped = new GetRatesResultDto
            {
                AccountNumber = getRates.AccountNumber,
                AccountCustomerNumber = customerDetails.AccountCustomerNumber,
                RateGroupCode = customerDetails.RateGroupCode,
                DeliveryAddress = new LocationDto
                {
                    AddressLine1      = getRates.DeliveryAddressLine1,
                    Suburb            = getRates.DeliverySuburb,
                    State             = getRates.DeliveryState,
                    PostCode          = getRates.DeliveryPostcode,
                    DepotAbbreviation = deliverLocation.DepotAbbreviation,
                    LocationType      = deliverLocation.LocationType.MapDestinationDescription(),
                    DepotName         = deliverLocation.ParAreaCode,
                },
                PickupAddress = new LocationDto
                {
                    AddressLine1      = getRates.PickupAddressLine1,
                    Suburb            = getRates.PickupSuburb,
                    State             = getRates.PickupState,
                    PostCode          = getRates.PickupPostcode,
                    DepotAbbreviation = pickLocation.DepotAbbreviation,
                    LocationType      = pickLocation.LocationType.MapDestinationDescription(),
                    DepotName         = pickLocation.ParAreaCode,
                },
                Vehicle = new VehicleQuoteRateDto
                {
                    VehicleValue = getRates.VehicleValue,
                    IsDriveable  = getRates.IsDriveable,
                    Model        = vehicleDetails.Model,
                    ModelCode    = vehicleDetails.ModelCode,
                    Make         = vehicleDetails.Make,
                    VehicleCode  = vehicleDetails.VehicleCode,
                    VehicleType  = vehicleDetails.Type,
                    Rates        = vehicleQuotes
                },
                IsExpired     = false,
                MarketingCode = 0
            };

            return new Result<GetRatesResultDto>(getRatesMapped);
        }
    }
}