using System.Diagnostics;

using FACTS.GenericBooking.Common.Configuration;
using FACTS.GenericBooking.Repository.Ingres.Entities;
using FACTS.GenericBooking.Repository.Ingres.IngresConfiguration;

using FluentNHibernate.Cfg;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;

namespace FACTS.GenericBooking.Api.ExtensionMethods.ServiceConfigurationExtensions
{
    public static class AddIngresConfigurationExtensions
    {
        public static void AddIngresNHibernateConfiguration(this IServiceCollection services, DatabaseConnections databaseConnections)
        {
            services.AddSingleton<ISessionFactory>(factory =>
            {
            #if DEBUG
                return Fluently
                    .Configure()
                    .Database(IngresOdbcConnection.CustomIngress.ConnectionString(databaseConnections.IngresDatabaseConnection).ShowSql)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AccountCustomerBusinessUnit>())
                    .ExposeConfiguration(c => c.SetInterceptor(new QueryInterceptor()))
                    .BuildConfiguration()
                    .BuildSessionFactory();
            #else
                return Fluently
                    .Configure()
                    .Database(IngresOdbcConnection.CustomIngress.ConnectionString(databaseConnections.IngresDatabaseConnection))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MakeType>())
                    .BuildConfiguration()
                    .BuildSessionFactory();
            #endif
            });
            services.AddScoped<NHibernate.ISession>(s => s.GetService<NHibernate.ISessionFactory>().OpenSession());
        }

        private class QueryInterceptor : EmptyInterceptor
        {
            public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
            {
                Debug.WriteLine(sql.ToString());
                return sql;
            }
        }
    }
}
