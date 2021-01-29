using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Models.Domain;
using FACTS.GenericBooking.Common.Models.Domain.Messages;
using FACTS.GenericBooking.Domain.Models.Customer;
using FACTS.GenericBooking.Domain.Services.Interfaces;

using NHibernate;
using NHibernate.Transform;

namespace FACTS.GenericBooking.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ISession _nhibernateSession;

        public CustomerService(ISession nhibernateSession)
        {
            _nhibernateSession = nhibernateSession;
        }

        public async Task<Result<CustomerDetailsDto>> GetAccountDetailsAsync(string username, string accountNumber)
        {
            string linkedAccountsSql =
                @"SELECT a.acc_cus_no           AS AccountCustomerNumber, 
                           a.cus_abrv           AS CustomerAbbreviation,
                           a.cus_alias          AS CustomerAlias, 
                           a.cus_name           AS CustomerName, 
                           a.def_phone_no       AS PhoneNumber, 
                           a.def_std_no         AS PhoneAreaCode, 
                           a.ins_waiver_ind     AS IsInsuranceWaiver, 
                           a.rate_group_code    AS RateGroupCode, 
                           a.disc_value         AS DiscountValue, 
                           a.value_code         AS ValueCode, 
                           a.veh_warranty_amt   AS VehicleWarrantyAmount, 
                           a.voice_alert_ind    AS IsVoiceAlert
                    FROM   web_acc_cus wa 
                           join acc_cus a on wa.acc_cus_no = a.acc_cus_no
                    WHERE  upper(login) = :login 
                          -- AND a.cus_status_code = 'ACT'";
            IList<CustomerDetailsDto> linkedAccounts = await _nhibernateSession.CreateSQLQuery(linkedAccountsSql)
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(CustomerDetailsDto)))
                .SetParameter("login", username)
                .ListAsync<CustomerDetailsDto>();

            if (linkedAccounts == null)
                return new Result<CustomerDetailsDto>(ErrorMessages.AccountDetailsNotFound);

            CustomerDetailsDto linkedAccount = linkedAccounts.FirstOrDefault(x => x.CustomerAlias == accountNumber);
            if (linkedAccount == null)
                return new Result<CustomerDetailsDto>(ErrorMessages.CustomerAccountDetailsNotFound(accountNumber));

            return new Result<CustomerDetailsDto>(linkedAccount);
        }
    }
}