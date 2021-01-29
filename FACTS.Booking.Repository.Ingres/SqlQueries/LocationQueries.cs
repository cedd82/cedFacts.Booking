namespace FACTS.GenericBooking.Repository.Ingres.SqlQueries
{
    public static class LocationQueries
    {
        public const string GetSuburbs =
            @"SELECT Trim(suburb)        AS Suburb, 
                       Trim(state)         AS State, 
                       postcode            AS PostCode, 
                       Trim(depot_abrv)    AS DepotAbbreviation, 
                       Trim(par_area_code) AS ParAreaCode, 
                       CASE 
                         WHEN Trim(location_type_code) = '' THEN 'customer' 
                         WHEN Trim(location_type_code) = 'CMP' THEN 'depot' 
                         WHEN Trim(location_type_code) = 'WHF' THEN 'wharf' 
                       END                 AS LocationType 
                FROM   suburb_type";

        public const string GetDepots =
            @"SELECT Trim(depot_abrv) AS DepotAbbreviation, 
                       Trim(web_name)   AS WebName, 
                       Trim(suburb)     AS Suburb, 
                       Trim(state)      AS State, 
                       Trim(postcode)   AS Postcode 
                FROM   depot 
                WHERE  bus_unit_code IN ( '', 'TCC' ) 
                       AND log_del_ind != 1 
                       AND web_name != '' 
                ORDER  BY web_name";
    }
}
