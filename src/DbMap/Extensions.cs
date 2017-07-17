using System.Data;
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

        public static object ExecuteScalar(this DbConnection @this, string commandText) =>
            Querying.ExecuteScalar(@this, commandText);

        public static object ExecuteScalar(this DbConnection @this, string commandText, object parameters) =>
            Querying.ExecuteScalar(@this, commandText, parameters);

        public static object ExecuteScalar(this DbConnection @this, string commandText, bool isStoredProcedure, object parameters) =>
            Querying.ExecuteScalar(@this, commandText, isStoredProcedure, parameters);
        
        public static T ExecuteScalar<T>(this DbConnection @this, string commandText) =>
            Querying.ExecuteScalar<T>(@this, commandText);
        
        public static T ExecuteScalar<T>(this DbConnection @this, string commandText, object parameters) =>
            Querying.ExecuteScalar<T>(@this, commandText, parameters);
        
        public static T ExecuteScalar<T>(this DbConnection @this, string commandText, bool isStoredProcedure, object parameters) =>
            Querying.ExecuteScalar<T>(@this, commandText, isStoredProcedure, parameters);

        public static DataTable ExecuteToDataTable(this DbConnection @this, , string commandText) =>
           Querying.ExecuteToDataTable(@this, commandText);

        public static DataTable ExecuteToDataTable(this DbConnection @this, , string commandText, object parameters) =>
            Querying.ExecuteToDataTable(@this, commandText, parameters);

        public static DataTable ExecuteToDataTable(this DbConnection @this, , string commandText, bool isStoredProcedure, object parameters) =>
            Querying.ExecuteToDataTable(@this, commandText, false, parameters);

    }

}
