﻿using System.Data.Common;
using System.Data;
using System.Linq;
using System.Collections.Generic;

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

        public static T ExecuteScalar<T>(DbConnection connection, string commandText) =>
            ExecuteScalar<T>(connection, commandText, false, null);

        public static T ExecuteScalar<T>(DbConnection connection, string commandText, object parameters) =>
            ExecuteScalar<T>(connection, commandText, false, parameters);

        public static T ExecuteScalar<T>(DbConnection connection, string commandText, bool isStoredProcedure, object parameters)
        {
            return
                Mapping.MapTo<T>(
                    ExecuteScalar(connection, commandText, isStoredProcedure, parameters)
                );
        }

        public static object ExecuteScalar(DbConnection connection, string commandText) =>
            ExecuteScalar(connection, commandText, false, null);

        public static object ExecuteScalar(DbConnection connection, string commandText, object parameters) =>
            ExecuteScalar(connection, commandText, false, parameters);
        
        public static object ExecuteScalar(DbConnection connection, string commandText, bool isStoredProcedure, object parameters)
        {
            using (var command = CreateCommand(connection, commandText, isStoredProcedure, parameters))
                return command.ExecuteScalar();
        }
        
        public static DataTable ExecuteToDataTable(DbConnection connection, string commandText) =>
            ExecuteToDataTable(connection, commandText, false, null);

        public static DataTable ExecuteToDataTable(DbConnection connection, string commandText, object parameters) =>
            ExecuteToDataTable(connection, commandText, false, parameters);

        public static DataTable ExecuteToDataTable(DbConnection connection, string commandText, bool isStoredProcedure, object parameters)
        {
            using (var command = CreateCommand(connection, commandText, isStoredProcedure, parameters))
            using (var adapter = DbProviderFactories.GetFactory(connection).CreateDataAdapter())
            {
                if (adapter == null) return null;
                var table = new DataTable();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                return table;
            }  
        }

        public static List<T> Execute<T>(DbConnection connection, string commandText) where T : new() =>
            Execute<T>(connection, commandText, false, null);

        public static List<T> Execute<T>(DbConnection connection, string commandText, object parameters) where T : new() =>
            Execute<T>(connection, commandText, false, parameters);

        public static List<T> Execute<T>(DbConnection connection, string commandText, bool isStoredProcedure, object parameters) where T : new()
        {
            using (var table = ExecuteToDataTable(connection, commandText, isStoredProcedure, parameters))
            {
                return (
                    from DataRow row 
                    in table.Rows
                    select Enumerable.Range(0, table.Columns.Count)
                        .Select(i => new KeyValuePair<string, object>(table.Columns[i].ColumnName, row[i])).ToList() 
                    into pairs
                    select Mapping.CreateObject<T>(pairs)
                ).ToList();
            }
        }
        
        private static DbCommand CreateCommand(DbConnection connection, string commandText, bool isStoredProcedure, object parameters = null)
        {

            if (connection.State != ConnectionState.Open) connection.Open();

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
