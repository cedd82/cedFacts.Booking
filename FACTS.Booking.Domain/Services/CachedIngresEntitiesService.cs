using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FACTS.GenericBooking.Domain.Models.IngresDto;
using FACTS.GenericBooking.Domain.Models.Vehicle;
using FACTS.GenericBooking.Domain.Services.Interfaces;
using FACTS.GenericBooking.Repository.Ingres.Entities;

using Microsoft.Extensions.Caching.Memory;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

namespace FACTS.GenericBooking.Domain.Services
{
    public class CachedIngresEntitiesService : ICachedIngresEntitiesService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ISession _nhibernateSession;

        public CachedIngresEntitiesService(ISession nhibernateSession,
                                           IMemoryCache memoryCache)
        {
            _nhibernateSession = nhibernateSession;
            _memoryCache       = memoryCache;
        }

        public async Task<IList<DepotDto>> GetDepotsCachedAsync()
        {
            IList<DepotDto> cacheEntry = await _memoryCache.GetOrCreateAsync("Depots", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                IQuery query = _nhibernateSession.CreateSQLQuery(@"SELECT Trim(depot_abrv) AS DepotAbbreviation, 
                               Trim(web_name)   AS WebName, 
                               Trim(suburb)     AS Suburb, 
                               Trim(state)      AS State, 
                               Trim(postcode)   AS Postcode 
                        FROM   depot 
                        WHERE  bus_unit_code IN ( '', 'TCC' ) 
                               AND log_del_ind != 1 
                               AND web_name != '' 
                        ORDER  BY web_name")
                    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(DepotDto)));
                IList<DepotDto> result = await query.ListAsync<DepotDto>();
                return result;
            });
            return cacheEntry;
        }

        public async Task<IList<SuburbDto>> GetSuburbsCachedAsync()
        {
            IList<SuburbDto> cacheEntry = await _memoryCache.GetOrCreateAsync("Suburbs", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                IQuery query = _nhibernateSession.CreateSQLQuery(@"SELECT Trim(suburb)        AS Suburb, 
                               Trim(state)         AS State, 
                               postcode            AS PostCode, 
                               Trim(depot_abrv)    AS DepotAbbreviation, 
                               Trim(par_area_code) AS ParAreaCode, 
                               CASE 
                                 WHEN Trim(location_type_code) = '' THEN 'customer' 
                                 WHEN Trim(location_type_code) = 'CMP' THEN 'depot' 
                                 WHEN Trim(location_type_code) = 'WHF' THEN 'wharf' 
                               END                 AS LocationType 
                        FROM   suburb_type")
                    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(SuburbDto)));
                IList<SuburbDto> result = await query.ListAsync<SuburbDto>();
                return result;
            });
            return cacheEntry;
        }

        public async Task<IList<VehicleTypeDto>> GetVehicleTypesCachedAsync()
        {
            IList<VehicleTypeDto> cacheEntry = await _memoryCache.GetOrCreateAsync("VehicleTypes", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                IQuery query = _nhibernateSession.CreateSQLQuery(@"SELECT makt.description        AS Make, 
                           makt.make_code           AS MakeCode, 
                           modt.description         AS Model, 
                           modt.model_code          AS ModelCode, 
                           vt.description           AS Type, 
                           vt.veh_code              AS VehicleCode, 
                           vct.veh_rate_code        AS RateCode,
                           vct.veh_rate_code_sec    AS RateCodeSecondary
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
                    ORDER  BY makt.description")
                    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(VehicleTypeDto)));
                IList<VehicleTypeDto> result = await query.ListAsync<VehicleTypeDto>();
                return result;
            });
            return cacheEntry;
        }

        public async Task<IList<RateGroupTypeDto>> GetRateGroupTypesCachedAsync()
        {
            IList<RateGroupTypeDto> cacheEntry = await _memoryCache.GetOrCreateAsync("RateGroupTypes", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                IQuery query = _nhibernateSession.CreateSQLQuery(@"SELECT rate_group_code       AS RateGroupCode, 
                       service_code        AS ServiceCode, 
                       description         AS Description, 
                       veh_value_limit     AS VehicleValueLimit, 
                       ins_excess_amt      AS InsuranceExcessAmount, 
                       veh_value_excess    AS VehicleValueExcess, 
                       nil_excess_chrg     AS NilExcessCharge, 
                       apply_sec_rate_code AS ApplySecondaryRateCode 
                FROM   rate_group_type")
                    .SetResultTransformer(new AliasToBeanResultTransformer(typeof(RateGroupTypeDto)));
                IList<RateGroupTypeDto> result = await query.ListAsync<RateGroupTypeDto>();
                return result;
            });
            return cacheEntry;
        }

        public async Task<IList<ApplicationRole>> GetApplicationRolesCachedAsync()
        {
            IList<ApplicationRole> cacheEntry = await _memoryCache.GetOrCreateAsync("ApplicationRoles", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                IQueryable<ApplicationRole> query = _nhibernateSession.Query<ApplicationRole>();
                List<ApplicationRole> result = await query.ToListAsync();
                return result;
            });
            return cacheEntry;
        }
    }
}