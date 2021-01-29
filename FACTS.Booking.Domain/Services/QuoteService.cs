using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using FACTS.GenericBooking.Common.Constants;
using FACTS.GenericBooking.Common.ExtensionMethods;
using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Domain.Models.Quote;
using FACTS.GenericBooking.Domain.Services.Interfaces;
using FACTS.GenericBooking.Repository.Ingres.SqlQueries;

using NHibernate;

using NLog;

namespace FACTS.GenericBooking.Domain.Services
{
    public class QuoteService : IQuoteService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ISession _nhibernateSession;
        private readonly IQuoteRateService _quoteRateService;
        private readonly IMapper _mapper;

        public QuoteService(ISession nhibernateSession, IQuoteRateService quoteRateService, IMapper mapper)
        {
            _nhibernateSession = nhibernateSession;
            _quoteRateService  = quoteRateService;
            _mapper       = mapper;
        }

        public async Task<Result<CreateQuoteResultDto>> CreateQuoteAsync(CreateQuoteDto model)
        {
            GetRatesDto getRate = _mapper.Map<CreateQuoteDto, GetRatesDto>(model);
            Result<GetRatesResultDto> quoteRateResult = await _quoteRateService.GetRatesAsync(getRate);
            if (quoteRateResult.Failure())
                return new Result<CreateQuoteResultDto>(quoteRateResult.ApiMessage);
            Result<int> createQuoteResult = await SaveQuoteAsync(quoteRateResult.Value, model);
            if (createQuoteResult.Failure())
                return new Result<CreateQuoteResultDto>(createQuoteResult.ApiMessage);
            CreateQuoteResultDto result = _mapper.Map<GetRatesResultDto, CreateQuoteResultDto>(quoteRateResult.Value);
            result.Vehicle.Rate.QuoteNumber = createQuoteResult.Value;
            return new Result<CreateQuoteResultDto>(result);
        }
        
        private async Task<Result<int>> SaveQuoteAsync(GetRatesResultDto get, CreateQuoteDto createQuote)
        {
            using ITransaction transaction = _nhibernateSession.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                int quoteNumber = await _nhibernateSession.CreateSQLQuery("select next value for seq_quote_no").UniqueResultAsync<int>();
                VehicleQuoteRateDto vehicleQuote = get.Vehicle;
                VehicleRateDto rate = vehicleQuote.Rates.First();
                await _nhibernateSession.CreateSQLQuery(QuoteSql.InsertQuoteLineMiscCharge)
                    .SetParameter("quoteNo", quoteNumber)
                    .SetParameter("quoteLineNo", 1)
                    .SetParameter("amount", vehicleQuote.VehicleValue)
                    .ExecuteUpdateAsync();

                await _nhibernateSession.CreateSQLQuery(QuoteSql.InsertQuoteLine)
                    .SetParameter("quoteNo", quoteNumber )
                    .SetParameter("isDriveable", vehicleQuote.IsDriveable ? 1 : 0)
                    .SetParameter("vehicleId", createQuote.VehicleId)
                    .SetParameter("vehicleValue", vehicleQuote.VehicleValue)
                    .SetParameter("transportCharge", rate.TransportCharge)
                    .SetParameter("insuranceCharge", rate.InsuranceCharge)
                    .SetParameter("surcharge", rate.Surcharge)
                    .SetParameter("miscCharge", rate.MiscCharge)
                    .SetParameter("vehicleRateCode", rate.VehicleRateCode)
                    .SetParameter("rateCode", rate.RateCode)
                    .SetParameter("rateRouteCode", rate.RateRouteCode)
                    .SetParameter("depot1", rate.PickupDepot)
                    .SetParameter("depot2", rate.DeliveryDepot)
                    .SetParameter("ringCode", rate.RingCode)
                    .SetParameter("serviceCode", rate.ServiceType.MapServiceType())
                    .SetParameter("vehTypeCode", vehicleQuote.VehicleCode)
                    .SetParameter("modelCode", vehicleQuote.ModelCode)
                    .ExecuteUpdateAsync();

                await _nhibernateSession.CreateSQLQuery(QuoteSql.InsertQuote)
                    .SetParameter("quote_no", quoteNumber)
                    .SetParameter("casualCustomerIndicator", get.AccountNumber == QuoteConsts.CasualCustomerAccountNumber)
                    .SetParameter("Title", createQuote.Title)
                    .SetParameter("FirstName", createQuote.FirstName)
                    .SetParameter("LastName", createQuote.LastName)
                    .SetParameter("LandlinePhoneAreaCode", createQuote.LandlinePhoneAreaCode)
                    .SetParameter("LandlinePhoneNumber", createQuote.LandlinePhoneNumber)
                    .SetParameter("MobileNumber", createQuote.MobileNumber)
                    .SetParameter("Email", createQuote.EmailAddress)
                    .SetParameter("PickAddressLine1", get.PickupAddress.AddressLine1)
                    .SetParameter("PickSuburb", get.PickupAddress.Suburb)
                    .SetParameter("PickState", get.PickupAddress.State)
                    .SetParameter("PickLocationType", get.PickupAddress.LocationType.MapDestinationType())
                    .SetParameter("DeliverAddressLine1", get.DeliveryAddress.AddressLine1)
                    .SetParameter("DeliverSuburb", get.DeliveryAddress.Suburb)
                    .SetParameter("DeliverState", get.DeliveryAddress.State)
                    .SetParameter("DeliverLocationType", get.DeliveryAddress.LocationType.MapDestinationType())
                    .SetParameter("tot_quote_chrg", rate.TotalRateExcludingGst)
                    .SetParameter("login", createQuote.Username)
                    .SetParameter("accCusNo", get.AccountCustomerNumber)
                    .SetParameter("rate_group_code", get.RateGroupCode)
                    .SetParameter("gst", rate.GST)
                    .SetParameter("mkt_code", get.MarketingCode)
                    .SetParameter("transit_days", rate.TransitDays)
                    .ExecuteUpdateAsync();

                Logger.Log(LogLevel.Info, $"saved quote: {quoteNumber}");
                await transaction.CommitAsync();
                return new Result<int>(quoteNumber);
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                Logger.Log(LogLevel.Error, exception, $"Error saving quote");
                return new Result<int>(ErrorMessages.UnableToCreateQuote);
            }
        }
    }
}