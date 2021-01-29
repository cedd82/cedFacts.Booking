using System.Data.Odbc;

using FACTS.GenericBooking.Domain.Models.Customer;
using FACTS.GenericBooking.Domain.Models.Quote;

namespace FACTS.GenericBooking.Domain.Helpers
{
    public static class OdbcHelper
    {
        public static OdbcParameter[] PopulateQuoteRateParams(GetRatesDto getRates,
                                                              CustomerDetailsDto customerDetails,
                                                              int vehicleRateCode,
                                                              string serviceCode,
                                                              string pickupType,
                                                              string deliveryType)
        {
            OdbcParameter[] odbcParams = new OdbcParameter[15];
            odbcParams[0]        = new OdbcParameter("p_pkup_suburb", OdbcType.VarChar);
            odbcParams[0].Value  = getRates.PickupSuburb;
            odbcParams[1]        = new OdbcParameter("p_pkup_state", OdbcType.VarChar);
            odbcParams[1].Value  = getRates.PickupState;
            odbcParams[2]        = new OdbcParameter("p_pkup_location_code", OdbcType.VarChar);
            odbcParams[2].Value  = pickupType;
            odbcParams[3]        = new OdbcParameter("p_dlvr_suburb", OdbcType.VarChar);
            odbcParams[3].Value  = getRates.DeliverySuburb;
            odbcParams[4]        = new OdbcParameter("p_dlvr_state", OdbcType.VarChar);
            odbcParams[4].Value  = getRates.DeliveryState;
            odbcParams[5]        = new OdbcParameter("p_dlvr_location_code", OdbcType.VarChar);
            odbcParams[5].Value  = deliveryType;
            odbcParams[6]        = new OdbcParameter("p_veh_rate_code", OdbcType.Int);
            odbcParams[6].Value  = vehicleRateCode;
            odbcParams[7]        = new OdbcParameter("p_acc_cus_no", OdbcType.Int);
            odbcParams[7].Value  = customerDetails.AccountCustomerNumber;
            odbcParams[8]        = new OdbcParameter("p_ins_waiver_ind", OdbcType.Int);
            odbcParams[8].Value  = customerDetails.IsInsuranceWaiver;
            odbcParams[9]        = new OdbcParameter("p_no_of_veh", OdbcType.Int);
            odbcParams[9].Value  = 1;
            odbcParams[10]       = new OdbcParameter("p_rate_group_code", OdbcType.VarChar);
            odbcParams[10].Value = customerDetails.RateGroupCode;
            odbcParams[11]       = new OdbcParameter("p_disc_value", OdbcType.Decimal);
            odbcParams[11].Value = customerDetails.DiscountValue;
            odbcParams[12]       = new OdbcParameter("p_value_code", OdbcType.VarChar);
            odbcParams[12].Value = customerDetails.ValueCode;
            odbcParams[13]       = new OdbcParameter("p_service_code", OdbcType.VarChar);
            odbcParams[13].Value = serviceCode;
            odbcParams[14]       = new OdbcParameter("p_driveable_ind", OdbcType.Int);
            odbcParams[14].Value = getRates.IsDriveable ? 1 : 0;
            return odbcParams;
        }
    }
}