using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Common.Constants;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Domain.Helpers;
using FACTS.GenericBooking.Domain.Mappers;
using FACTS.GenericBooking.Domain.Models.Customer;
using FACTS.GenericBooking.Domain.Models.IngresDto;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Models.Vehicle;
using FACTS.GenericBooking.Domain.Services.Interfaces;
using FACTS.GenericBooking.Repository.Ingres;
using FACTS.GenericBooking.Repository.Postgres.Entities;

using NLog;

namespace FACTS.GenericBooking.Domain.Services
{
    public class QuoteRateService : IQuoteRateService
    {
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IVehicleService _vehicleService;
        private readonly IIngresAccessService _ingresAccessService;
        private readonly CommonAppSettings _commonAppSettings;
        private readonly IProcessEventService _processEventService;

        public QuoteRateService(IVehicleService vehicleService,
                                IIngresAccessService ingresAccessService,
                                ICustomerService customerService,
                                ILocationService locationService,
                                CommonAppSettings commonAppSettings,
                                IProcessEventService processEventService)
        {
            _vehicleService      = vehicleService;
            _ingresAccessService = ingresAccessService;
            _customerService     = customerService;
            _locationService     = locationService;
            _commonAppSettings   = commonAppSettings;
            _processEventService = processEventService;
        }

        public async Task<Result<GetRatesResultDto>> GetRatesAsync(GetRatesDto getRates)
        {
            string logMessage = $@"GetRates for accountNumber:{getRates.AccountNumber} 
                        ServiceCode: {getRates.ServiceType} PickupType: {getRates.PickupType} DeliveryType:{getRates.DeliveryType}
                        PickupAddress: {getRates.PickupSuburb} {getRates.PickupState} {getRates.PickupPostcode}
                        DeliverAddress: {getRates.DeliverySuburb} {getRates.DeliveryState} {getRates.DeliveryPostcode}";
            Logger.Log(LogLevel.Info , $"Start: {logMessage}");
            
            ProcessEventData processEventData = await _processEventService.AddProcessEventDataAsync(getRates, getRates.Username, "GetRates", "get-rates");

            Result<CustomerDetailsDto> accountDetailsResult = await _customerService.GetAccountDetailsAsync(getRates.Username, getRates.AccountNumber);
            if (accountDetailsResult.Failure())
                return new Result<GetRatesResultDto>(accountDetailsResult.ApiMessage);
            CustomerDetailsDto accountDetails = accountDetailsResult.Value;

            Result<SuburbDto> pickLocationResult = await _locationService.GetSuburbAsync(AddressConsts.Pick, getRates.PickupSuburb, getRates.PickupPostcode, getRates.PickupState);
            if (pickLocationResult.Failure())
            {
                await _processEventService.UpdateProcessEventDataToErrorAndLogAsync(processEventData, pickLocationResult.ToString());
                return new Result<GetRatesResultDto>(pickLocationResult.ApiMessage);
            }

            Result<SuburbDto> deliverLocationResult = await _locationService.GetSuburbAsync(AddressConsts.Deliver, getRates.DeliverySuburb, getRates.DeliveryPostcode, getRates.DeliveryState);
            if (deliverLocationResult.Failure())
            {
                await _processEventService.UpdateProcessEventDataToErrorAndLogAsync(processEventData, deliverLocationResult.ToString());
                return new Result<GetRatesResultDto>(deliverLocationResult.ApiMessage);
            }

            Result<VehicleDetailsDto> vehicleDetailsResult = await _vehicleService.GetVehicleDetailsAsync(getRates, accountDetails.RateGroupCode);
            if (vehicleDetailsResult.Failure())
            {
                await _processEventService.UpdateProcessEventDataToErrorAndLogAsync(processEventData, vehicleDetailsResult.ToString());
                return new Result<GetRatesResultDto>(vehicleDetailsResult.ApiMessage);
            }

            // for each service code, pickupType and deliveryType, get a quote for each
            Result<List<VehicleRateDto>> vehicleQuotesResult = await GetVehicleRatesAsync(getRates, accountDetails, vehicleDetailsResult.Value);
            if (vehicleQuotesResult.Failure())
            {
                await _processEventService.UpdateProcessEventDataToErrorAndLogAsync(processEventData, vehicleQuotesResult.ToString());
                return new Result<GetRatesResultDto>(vehicleQuotesResult.ApiMessage);
            }

            List<VehicleRateDto> vehicleQuotes = vehicleQuotesResult.Value;
            SuburbDto pickLocation = pickLocationResult.Value;
            SuburbDto deliverLocation = deliverLocationResult.Value;
            
            Result<GetRatesResultDto> quotesResult = QuoteRateResultMap.MapQuoteRates(vehicleQuotes, getRates, vehicleDetailsResult.Value, deliverLocation, pickLocation, accountDetails);
            if (quotesResult.Failure())
            {
                await _processEventService.UpdateProcessEventDataToErrorAndLogAsync(processEventData, quotesResult.ToString());
                return new Result<GetRatesResultDto>(ErrorMessages.TransportChargesCouldNotBeDetermined);
            }
            
            await _processEventService.UpdateProcessEventDataCompleteAsync(processEventData, quotesResult.Value);
            Logger.Info( $"End Success: {logMessage}");
            return new Result<GetRatesResultDto>(quotesResult.Value);
        }

        private async Task<Result<List<VehicleRateDto>>> GetVehicleRatesAsync(GetRatesDto getRates, CustomerDetailsDto customerDetails, VehicleDetailsDto vehicleDetails)
        {
            List<string> serviceCodes = new();
            if (string.IsNullOrEmpty(getRates.ServiceType))
                serviceCodes.AddRange(new List<string>{ServiceType.Standard, ServiceType.Express, ServiceType.PremiumEnclosed });
            else
                serviceCodes.Add(getRates.ServiceType);
            
            List<RateLocationTypeDto> locationTypes = new();
            if (string.IsNullOrEmpty(getRates.PickupType) && string.IsNullOrEmpty(getRates.DeliveryType))
            {
                locationTypes.Add(new RateLocationTypeDto{Pickup = LocationType.Depot, Delivery = LocationType.Depot});
                locationTypes.Add(new RateLocationTypeDto{Pickup = LocationType.Customer, Delivery = LocationType.Customer});
                locationTypes.Add(new RateLocationTypeDto{Pickup = LocationType.Depot, Delivery = LocationType.Customer});
                locationTypes.Add(new RateLocationTypeDto{Pickup = LocationType.Customer, Delivery = LocationType.Depot});
            }
            else if (string.IsNullOrEmpty(getRates.PickupType))
            {
                locationTypes.Add(new RateLocationTypeDto{Pickup = LocationType.Depot, Delivery    = getRates.DeliveryType});
                locationTypes.Add(new RateLocationTypeDto{Pickup = LocationType.Customer, Delivery = getRates.DeliveryType});
            }
            else if (string.IsNullOrEmpty(getRates.DeliveryType))
            {
                locationTypes.Add(new RateLocationTypeDto{Pickup = getRates.PickupType, Delivery    = LocationType.Depot});
                locationTypes.Add(new RateLocationTypeDto{Pickup = getRates.PickupType, Delivery = LocationType.Customer});
            }
            else
            {
                locationTypes.Add(new RateLocationTypeDto{Pickup = getRates.PickupType, Delivery = getRates.DeliveryType});
            }

            List<VehicleRateDto> vehicleQuotes = new();
            foreach (string serviceCode in serviceCodes)
            {
                int vehicleRateCode = await _vehicleService.GetVehicleRateCode(vehicleDetails, customerDetails.RateGroupCode, serviceCode);
                foreach (RateLocationTypeDto locationType in locationTypes)
                {
                    Result<decimal> insuranceExcessResult = await _vehicleService.CalculateInsuranceExcessFromVehicleValue(serviceCode, customerDetails.RateGroupCode, vehicleDetails.VehicleValue);
                    if (insuranceExcessResult.Failure())
                        continue;

                    OdbcParameter[] quoteRateParams = OdbcHelper.PopulateQuoteRateParams(getRates, customerDetails, vehicleRateCode, serviceCode, locationType.Pickup, locationType.Delivery);
                    string ingresQuoteRateRaw = await _ingresAccessService.ExecuteStoredProcedureAsync("quote_rate_dbp", quoteRateParams);
                    
                    if (string.IsNullOrEmpty(ingresQuoteRateRaw) || !ingresQuoteRateRaw.Contains("##"))
                        continue;

                    string movementType = await _locationService.GetMovementTypeAsync(getRates.PickupSuburb, getRates.PickupState, getRates.DeliverySuburb, getRates.DeliveryState);

                    Result<VehicleRateDto> vehicleQuoteResult = VehicleQuoteRateMap.Map(ingresQuoteRateRaw, 
                                                                                         _commonAppSettings, 
                                                                                         serviceCode, 
                                                                                         locationType,
                                                                                         insuranceExcessResult.Value,
                                                                                         vehicleRateCode,
                                                                                         null,
                                                                                         movementType);
                    if (vehicleQuoteResult.Failure())
                        return new Result<List<VehicleRateDto>>(vehicleQuoteResult.ApiMessage);
                    vehicleQuotes.Add(vehicleQuoteResult.Value);
                }
            }

            return !vehicleQuotes.Any() 
                ? new Result<List<VehicleRateDto>>(ErrorMessages.UnableToGetRatesForRateGroupAndServiceType) 
                : new Result<List<VehicleRateDto>>(vehicleQuotes);
        }   
    }
}