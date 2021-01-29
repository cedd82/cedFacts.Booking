namespace FACTS.GenericBooking.Repository.Ingres.SqlQueries
{
    public static class CustomerQueries
    {
        public const string CustomerDetailsFromAccountNumber = 
            @"SELECT acc_cus_no     AS AccountNumber, 
                   cus_abrv         AS CustomerAbbreviation,
                   cus_alias        AS CustomerAlias, 
                   cus_name         AS CustomerName, 
                   def_phone_no     AS PhoneNumber, 
                   def_std_no       AS PhoneAreaCode, 
                   ins_waiver_ind   AS IsInsuranceWaiver, 
                   rate_group_code  AS RateGroupCode, 
                   disc_value       AS DiscountValue, 
                   value_code       AS ValueCode, 
                   veh_warranty_amt AS VehicleWarrantyAmount,
                   voice_alert_ind  AS IsVoiceAlert
            FROM   acc_cus 
            WHERE  acc_cus_no = :accountNumber 
                   AND log_del_ind = 0 ";

        public const string CustomerDetailsFromCustomerAlias = 
            @"SELECT acc_cus_no     AS AccountNumber, 
                   cus_abrv         AS CustomerAbbreviation,
                   cus_alias        AS CustomerAlias, 
                   cus_name         AS CustomerName, 
                   def_phone_no     AS PhoneNumber, 
                   def_std_no       AS PhoneAreaCode, 
                   ins_waiver_ind   AS IsInsuranceWaiver, 
                   rate_group_code  AS RateGroupCode, 
                   disc_value       AS DiscountValue, 
                   value_code       AS ValueCode, 
                   veh_warranty_amt AS VehicleWarrantyAmount,
                   voice_alert_ind  AS IsVoiceAlert
            FROM   acc_cus 
            WHERE  cus_alias = :customerAlias 
                   AND log_del_ind = 0 ";
    }
}
