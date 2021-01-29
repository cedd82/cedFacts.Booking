﻿namespace FACTS.GenericBooking.Repository.Ingres.SqlQueries
{
    public static class QuoteSql
    {
        public const string InsertQuoteLineMiscCharge =
            @"INSERT INTO quote_line_misc_chrg
                        (quote_no,
                         quote_line_no,
                         misc_chrg_code,
                         amount)
            VALUES      (:quoteNo,
                         :quoteLineNo,
                         'NDR',
                         :amount) ";

        public const string InsertQuoteLine =
            @"INSERT INTO quote_line
                        (quote_no,
                         quote_line_no,
                         driveable_ind,
                         nil_excess_ins_ind,
                         veh_id,
                         veh_qty,
                         veh_value,
                         lhl_chrg,
                         ins_chrg,
                         calc_fuel_surchrg,
                         tot_misc_chrg,
                         veh_rate_code,
                         rate_code,
                         rate_route_code,
                         depot_abrv1,
                         depot_abrv2,
                         dist_rng_code,
                         service_code,
                         veh_code,
                         model_code)
            VALUES      (:quoteNo,
                         1,
                         :isDriveable,
                         1,
                         :vehicleId,
                         1,
                         :vehicleValue,
                         :transportCharge,
                         :insuranceCharge,
                         :surcharge,
                         :miscCharge,
                         :vehicleRateCode,
                         :rateCode,
                         :rateRouteCode,
                         :depot1,
                         :depot2,
                         :ringCode,
                         :serviceCode,
                         :vehTypeCode,
                         :modelCode) ";

        public const string InsertQuote =
            @"INSERT INTO quote
                        (quote_no,
                         quote_status_code,
                         status_effect_date,
                         cas_cus_ind,
                         cont_title,
                         cont_first_name,
                         cont_surname,
                         cont_std_no,
                         cont_phone_no,
                         cont_mobile_no,
                         email_id,
                         pkup_street,
                         pkup_suburb,
                         pkup_state,
                         pkup_location_code,
                         dlvr_street,
                         dlvr_suburb,
                         dlvr_state,
                         dlvr_location_code,
                         veh_count,
                         tot_quote_chrg,
                         operator_id,
                         web_login,
                         quote_tms,
                         acc_cus_no,
                         rate_group_code,
                         gst_chrg,
                         mkt_code,
                         web_ind,
                         transit_days)
            VALUES      ( :quote_no,
                          'OPN',
                          Date('now'),
                          :casualCustomerIndicator,
                          :Title,
                          :FirstName,
                          :LastName,
                          :LandlinePhoneAreaCode,
                          :LandlinePhoneNumber,
                          :MobileNumber,
                          :Email,
                          :PickAddressLine1,
                          :PickSuburb,
                          :PickState,
                          :PickLocationType,
                          :DeliverAddressLine1,
                          :DeliverSuburb,
                          :DeliverState,
                          :DeliverLocationType,
                          1,
                          :tot_quote_chrg,
                          'SYSTEM',
                          :login,
                          Date('now'),
                          :accCusNo,
                          :rate_group_code,
                          :gst,
                          :mkt_code,
                          1,
                          :transit_days ) ";


    }
}