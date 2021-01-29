using AutoMapper;

using FACTS.GenericBooking.Api.Models.Auth;
using FACTS.GenericBooking.Api.Models.Quote;
using FACTS.GenericBooking.Api.Models.Vehicle;
using FACTS.GenericBooking.Common.ExtensionMethods;
using FACTS.GenericBooking.Domain.Models.Auth;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Models.Vehicle;

namespace FACTS.GenericBooking.Api.Mappers
{
    public class ApiAutomapperProfile : Profile
    {
        public ApiAutomapperProfile()
        {
            // auth
            CreateMap<AuthenticateUserRequest, AuthenticateUserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Trim().ToUpper()));
            CreateMap<CreateUserRequest, CreateUserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Trim().ToUpper()));
            CreateMap<UpdateUserRequest, UpdateUserDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Trim().ToUpper()));
            CreateMap<UpdatePasswordRequest, UpdatePasswordDto>();
            CreateMap<UserLoginDto, AuthenticateUserResponse>();

            //vehicle
            CreateMap<GetVehicleTypesRequest, GetVehicleTypeDto>()
                .ForMember(d => d.Model, o => o.MapFrom(s => s.Model.Trim().ToUpper()))
                .ForMember(d => d.Make, o => o.MapFrom(s => s.Make.Trim().ToUpper()))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.Trim().ToUpper()));
            CreateMap<VehiclesByMakeDto, GetVehiclesByMakeResponse>();
            CreateMap<VehicleTypeDto, VehicleModelResponse>();
            CreateMap<VehicleTypeDto, GetVehiclesByMakeResponse>();
            
            // rates
            CreateMap<GetRatesRequest, GetRatesDto>()
                .ForMember(d => d.AccountNumber, o => o.MapFrom(s => string.IsNullOrEmpty(s.AccountNumber) ? null : s.AccountNumber.Trim().ToUpper()))
                .ForMember(d => d.ServiceType, o => o.MapFrom(s => s.ServiceType.MapServiceType()))
                .ForMember(d => d.PickupType, o => o.MapFrom(s => s.PickupType.MapDestinationType()))
                .ForMember(d => d.PickupSuburb, o => o.MapFrom(s => s.PickupSuburb.Trim().ToUpper()))
                .ForMember(d => d.PickupPostcode, o => o.MapFrom(s => s.PickupPostcode.Trim()))
                .ForMember(d => d.PickupState, o => o.MapFrom(s => s.PickupState.ToUpper()))
                .ForMember(d => d.DeliveryType, o => o.MapFrom(s => s.DeliveryType.MapDestinationType()))
                .ForMember(d => d.DeliverySuburb, o => o.MapFrom(s => s.DeliverySuburb.Trim().ToUpper()))
                .ForMember(d => d.DeliveryPostcode, o => o.MapFrom(s => s.DeliveryPostcode.Trim().ToUpper()))
                .ForMember(d => d.DeliveryState, o => o.MapFrom(s => s.DeliveryState.Trim().ToUpper()))
                .ForMember(d => d.IsDriveable, o => o.MapFrom(s => s.IsDriveable.Value))
                .ForMember(d => d.IsModified, o => o.MapFrom(s => s.IsModified.Value))
                .ForMember(d => d.IsDamaged, o => o.MapFrom(s => s.IsDamaged.Value))
                .ForMember(d => d.VehicleMake, o => o.MapFrom(s => s.VehicleMake.Trim().ToUpper()))
                .ForMember(d => d.VehicleModel, o => o.MapFrom(s => s.VehicleModel.Trim().ToUpper()))
                .ForMember(d => d.VehicleType, o => o.MapFrom(s => s.VehicleType.Trim().ToUpper()));

            CreateMap<GetRatesResultDto, GetRatesResponse>();
            CreateMap<LocationDto, LocationResponse>();
            CreateMap<VehicleQuoteRateDto, VehicleRateResponse>();
            CreateMap<VehicleRateDto, RateResponse>();
            //VehicleRateResponse VehicleQuoteRateDto
            CreateMap<CreateQuoteRequest, CreateQuoteDto>()
                .ForMember(d => d.AccountNumber, o => o.MapFrom(s => string.IsNullOrEmpty(s.AccountNumber) ? null : s.AccountNumber.Trim().ToUpper()))
                .ForMember(d => d.ServiceType, o => o.MapFrom(s => s.ServiceType.MapServiceType()))
                .ForMember(d => d.PickupType, o => o.MapFrom(s => s.PickupType.MapDestinationType()))
                .ForMember(d => d.PickupAddressLine1, o => o.MapFrom(s => s.PickupAddressLine1.Trim().ToUpper()))
                .ForMember(d => d.PickupSuburb, o => o.MapFrom(s => s.PickupSuburb.Trim().ToUpper()))
                .ForMember(d => d.PickupPostcode, o => o.MapFrom(s => s.PickupPostcode.Trim()))
                .ForMember(d => d.PickupState, o => o.MapFrom(s => s.PickupState.ToUpper()))
                .ForMember(d => d.DeliveryType, o => o.MapFrom(s => s.DeliveryType.MapDestinationType()))
                .ForMember(d => d.DeliveryAddressLine1, o => o.MapFrom(s => s.DeliveryAddressLine1.Trim().ToUpper()))
                .ForMember(d => d.DeliverySuburb, o => o.MapFrom(s => s.DeliverySuburb.Trim().ToUpper()))
                .ForMember(d => d.DeliveryPostcode, o => o.MapFrom(s => s.DeliveryPostcode.Trim().ToUpper()))
                .ForMember(d => d.DeliveryState, o => o.MapFrom(s => s.DeliveryState.Trim().ToUpper()))
                .ForMember(d => d.IsDriveable, o => o.MapFrom(s => s.IsDriveable.Value))
                .ForMember(d => d.IsModified, o => o.MapFrom(s => s.IsModified.Value))
                .ForMember(d => d.IsDamaged, o => o.MapFrom(s => s.IsDamaged.Value))
                .ForMember(d => d.VehicleMake, o => o.MapFrom(s => s.VehicleMake.Trim().ToUpper()))
                .ForMember(d => d.VehicleModel, o => o.MapFrom(s => s.VehicleModel.Trim().ToUpper()))
                .ForMember(d => d.VehicleType, o => o.MapFrom(s => s.VehicleType.Trim().ToUpper()))
                .ForMember(d => d.VehicleValue, o => o.MapFrom(s => s.VehicleValue.Value))
                .ForMember(d => d.VehicleId, o => o.MapFrom(s => s.VehicleId.Trim()))
                .ForMember(d => d.Title, o => o.MapFrom(s => string.IsNullOrEmpty(s.Title) ? null : s.Title.Trim().ToUpper()))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => string.IsNullOrEmpty(s.FirstName) ? null : s.FirstName.Trim().ToUpper()))
                .ForMember(d => d.LastName, o => o.MapFrom(s => string.IsNullOrEmpty(s.LastName) ? null : s.LastName.Trim().ToUpper()))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName.Trim().ToUpper()))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.EmailAddress.Trim().ToUpper()));
            CreateMap<CreateQuoteResultDto, CreateQuoteResponse>();
            CreateMap<ContactDto, ContactResponse>();
            CreateMap<VehicleQuoteDto, VehicleQuoteResponse>();
            CreateMap<VehicleRateDto, QuoteRateResponse>()
                .ForMember(d => d.QuoteNumber, o => o.MapFrom(s => s.QuoteNumber.Value));
        }
    }
}