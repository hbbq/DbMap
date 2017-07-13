using System.Data.Common;

namespace DbMap
{

    public static class Extensions
    {

        public static int ExecuteNonQuery(this DbConnection @this, string commandText) =>
            Querying.ExecuteNonQuery(@this, commandText);

        public static int ExecuteNonQuery(this DbConnection @this, string commandText, object parameters) =>
            Querying.ExecuteNonQuery(@this, commandText, parameters);

        public static int ExecuteNonQuery(this DbConnection @this, string commandText, bool isStoredProcedure, object parameters) =>
            Querying.ExecuteNonQuery(@this, commandText, isStoredProcedure, parameters);

        public static DbDataReader ExecuteReader(this DbConnection @this, string commandText) =>
            Querying.ExecuteReader(@this, commandText);
        
        public static DbDataReader ExecuteReader(this DbConnection @this, string commandText, object parameters) =>
            Querying.ExecuteReader(@this, commandText, parameters);
        
        public static DbDataReader ExecuteReader(this DbConnection @this, string commandText, bool isStoredProcedure, object parameters) =>
            Querying.ExecuteReader(@this, commandText, isStoredProcedure, parameters);

    }

}
