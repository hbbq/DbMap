using System.Data.Common;
using System.Data;
using System.Linq;

namespace DbMap
{

    public static class Querying
    {
        
        public static int ExecuteNonQuery(DbConnection connection, string commandText) =>
            ExecuteNonQuery(connection, commandText, false, null);

        public static int ExecuteNonQuery(DbConnection connection, string commandText, object parameters) =>
            ExecuteNonQuery(connection, commandText, false, parameters);

        public static int ExecuteNonQuery(DbConnection connection, string commandText, bool isStoredProcedure, object parameters)
        {
            using (var command = CreateCommand(connection, commandText, isStoredProcedure, parameters))
                return command.ExecuteNonQuery();
        }

        public static DbDataReader ExecuteReader(DbConnection connection, string commandText) =>
            ExecuteReader(connection, commandText, false, null);
        
        public static DbDataReader ExecuteReader(DbConnection connection, string commandText, object parameters) =>
            ExecuteReader(connection, commandText, false, parameters);

        public static DbDataReader ExecuteReader(DbConnection connection, string commandText, bool isStoredProcedure, object parameters)
        {
            using (var command = CreateCommand(connection, commandText, isStoredProcedure, parameters))
                return command.ExecuteReader();
        }

        private static DbCommand CreateCommand(DbConnection connection, string commandText, bool isStoredProcedure, object parameters = null)
        {

            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            if(parameters != null)
            {
                foreach(var prop in parameters.GetType().GetProperties().Where(p => p.CanRead))
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = prop.Name;
                    parameter.Value = prop.GetValue(parameters);
                    command.Parameters.Add(parameter);
                }
            }

            return command;

        }

    }

}
