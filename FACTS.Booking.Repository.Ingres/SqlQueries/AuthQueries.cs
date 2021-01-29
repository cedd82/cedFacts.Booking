namespace FACTS.GenericBooking.Repository.Ingres.SqlQueries
{
    public class AuthQueries
    {
        public const string GetUserLogin =
            @"SELECT TRIM(o.operator_id)  AS OperatorId, 
                       o.group_id         AS GroupId, 
                       ob.is_enabled      AS IsEnabled,
                       o.operator_name    AS OperatorName,
                       o.email_addr       AS EmailAddress,
                       o.mobile_no        AS MobileNumber,
                       op.password_hash   AS PasswordHash, 
                       op.password_salt   AS PasswordSalt, 
                       op.bad_login_count AS BadLoginCount, 
                       op.lock_ind        AS IsLocked 
            FROM   operator o 
                   JOIN operator_bus_unit ob 
                     ON ob.operator_id = o.operator_id 
                   JOIN operator_password op 
                     ON op.operator_id = o.operator_id 
            WHERE  ob.bus_unit_code = op.bus_unit_code 
                   AND ob.bus_unit_code = 'CCC' 
                   AND ob.is_enabled = 1
                   AND ob.operator_id = :operatorId";
    }
}