using System.Data;
using System.Data.Odbc;
using System.Threading.Tasks;

using FACTS.GenericBooking.Common.Configuration;

namespace FACTS.GenericBooking.Repository.Ingres
{
    public interface IIngresAccessService
    {
        Task<string> ExecuteStoredProcedureAsync(string procedure, OdbcParameter[] odbcParameters);
    }

    public class IngresAccessService : IIngresAccessService
    {
        private readonly DatabaseConnections _databaseConnections;

        public IngresAccessService(DatabaseConnections databaseConnections) => _databaseConnections = databaseConnections;

        public async Task<string> ExecuteStoredProcedureAsync(string procedure, OdbcParameter[] odbcParameters)
        {
            string output = null;
            await using OdbcConnection connection = new(_databaseConnections.IngresDatabaseConnection);
            await connection.OpenAsync();
            connection.InfoMessage += (sender, args) => output += args.Message;
            string paramList = string.Empty;
            for (int i = 0; i < odbcParameters.Length; i++)
            {
                if (i == 0)
                    paramList = "?";
                else
                    paramList += ",?";
            }
            OdbcCommand oc = new($"{{?=call {procedure}({paramList})}}", connection);
            OdbcParameter opRetValue = new("return_val", OdbcType.Int)
            {
                Direction = ParameterDirection.ReturnValue
            };
            oc.Parameters.Add(opRetValue);
            foreach (OdbcParameter odp in odbcParameters)
            {
                oc.Parameters.Add(odp);
            }
            await oc.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            return output;
        }
    }
}
