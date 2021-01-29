namespace FACTS.GenericBooking.Repository.Ingres.SqlQueries
{
    public static class VehicleQueries
    {
        public const string VehicleTypes = 
            @"SELECT makt.description               AS VehicleMake, 
                           makt.make_code           AS MakeCode, 
                           modt.description         AS VehicleModel, 
                           modt.model_code          AS ModelCode, 
                           vt.description           AS VehicleType, 
                           vt.veh_code              AS VehicleCode, 
                           vct.veh_rate_code        AS VehicleRateCode,
                           vct.veh_rate_code_sec    AS VehicleRateCodeSecondary
                    FROM   veh_type vt 
                           JOIN veh_class_type vct 
                             ON vt.veh_code = vct.veh_code 
                                AND vt.log_del_ind = 0 
                                AND vct.log_del_ind = 0 
                           JOIN model_type modt 
                             ON vct.model_code = modt.model_code 
                                AND modt.log_del_ind = 0 
                           JOIN make_type makt 
                             ON modt.make_code = makt.make_code 
                                AND makt.log_del_ind = 0 
                    WHERE  makt.description <> '' 
                    ORDER  BY makt.description";

        public const string RateGroupTypes =
            @"SELECT rate_group_code       AS RateGroupCode, 
                       service_code        AS ServiceCode, 
                       description         AS Description, 
                       veh_value_limit     AS VehicleValueLimit, 
                       ins_excess_amt      AS InsuranceExcessAmount, 
                       veh_value_excess    AS VehicleValueExcess, 
                       nil_excess_chrg     AS NilExcessCharge, 
                       apply_sec_rate_code AS ApplySecondaryRateCode 
                FROM   rate_group_type";
    }
}
