using FluentNHibernate.Cfg.Db;

using NHibernate.Driver;

namespace FACTS.GenericBooking.Repository.Ingres.IngresConfiguration
{
    public class IngresOdbcConnection : PersistenceConfiguration<IngresOdbcConnection, OdbcConnectionStringBuilder>
    {
        private IngresOdbcConnection()
        {
            Driver<OdbcDriver>();
        }
        
        public static IngresOdbcConnection CustomIngress
        {
            get
            {
                IngresOdbcConnection ingresOdbcConnection = new IngresOdbcConnection().Dialect<NHibernate.Dialect.Ingres9Dialect>();
            #if DEBUG
                ingresOdbcConnection.ShowSql();
                ingresOdbcConnection.FormatSql();
            #endif
                return ingresOdbcConnection;
            }
        }
    }
}