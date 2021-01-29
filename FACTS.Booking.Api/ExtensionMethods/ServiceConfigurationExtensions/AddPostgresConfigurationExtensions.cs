using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Repository.Postgres;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FACTS.GenericBooking.Api.ExtensionMethods.ServiceConfigurationExtensions
{
    public static class AddPostgresConfigurationExtensions
    {
        public static void AddPostgresDbContext(this IServiceCollection services,
                                                DatabaseConnections databaseConnections,
                                                ILoggerFactory loggerFactory,
                                                bool isDevelopment)
        {
            services.AddDbContext<FactsDbContext>(options =>
            {
                options.UseNpgsql(databaseConnections.PostgresConnection);
                EventId[] events = {new(7777, "in memory linq execution in Facts booking")};
                options.UseLoggerFactory(loggerFactory);
                if (isDevelopment)
                {
                    options.EnableSensitiveDataLogging();
                    options.ConfigureWarnings(x => x.Throw(events));
                }
                else
                    options.ConfigureWarnings(x => x.Log(events));
            });
        }
    }
}