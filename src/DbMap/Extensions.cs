using System.Data.Common;

namespace DbMap
{

    public static class Extensions
    {

        public static int ExecuteNonQuery(this DbConnection @this, string commandText) =>
            Querying.ExecuteNonQuery(@this, commandText, false, null);

        public static int ExecuteNonQuery(this DbConnection @this, string commandText, object parameters) =>
            Querying.ExecuteNonQuery(@this, commandText, false, parameters);

        public static int ExecuteNonQuery(this DbConnection @this, string commandText, bool isStoredProcedure, object parameters) =>
            Querying.ExecuteNonQuery(@this, commandText, isStoredProcedure, parameters);

    }

}
