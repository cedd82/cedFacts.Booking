using System.Linq;

using AutoMapper;

using FACTS.GenericBooking.Domain.Models.Auth;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Models.Vehicle;

// ReSharper disable EmptyConstructor

namespace FACTS.GenericBooking.Domain.Mappers
{
    public class DomainAutomapperProfile : Profile
    {
        public DomainAutomapperProfile()
        {
            CreateMap<VehicleTypeDto, VehicleDetailsDto>();
            CreateMap<UserLoginDetailsDto, UserLoginDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.OperatorId.Trim().ToUpper()));

            // create quote
            CreateMap<CreateQuoteDto, GetRatesDto>();
            CreateMap<GetRatesResultDto, CreateQuoteResultDto>();
            CreateMap<VehicleQuoteRateDto, VehicleQuoteDto>()
                .ForMember(d => d.Rate, o => o.MapFrom(s => s.Rates.First()));

        }
    }
}
