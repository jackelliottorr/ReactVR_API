using System;
using System.Linq;
using System.Reflection;
using System.Data.SqlClient;
using Dapper;

namespace ReactVR_API.Core.HelperClasses
{
    public static class SqlCrudHelper
    {
        public static string GetInsertStatement(object obj, string tableName, string primaryKeyColumn)
        {
            string sbCols;
            string sbValues;
            Type _type = obj.GetType();
            PropertyInfo[] properties = _type.GetProperties();
            sbCols = string.Join(", ", properties.Where(x => x.Name != primaryKeyColumn).Select(x => $"[{x.Name}]"));
            sbValues = string.Join(", ", properties.Where(x => x.Name != primaryKeyColumn).Select(x => $"@{x.Name}"));

            string sQuery = $"INSERT {tableName} ({sbCols}) OUTPUT Inserted.{primaryKeyColumn} VALUES ({sbValues});";

            return sQuery;
        }

        public static string GetDeleteStatement(string tableName, string primaryKeyColumn)
        {
            string sQuery = $"DELETE FROM {tableName} WHERE {primaryKeyColumn} = @{primaryKeyColumn}";

            return sQuery;
        }

        public static string GetUpdateStatement(object obj, string tableName)
        {
            string sbCols;
            Type _type = obj.GetType();
            PropertyInfo[] properties = _type.GetProperties(); 
            sbCols = string.Join(", ", properties.Select(x => $"[{x.Name}] = @{x.Name}"));
            
            string sQuery = $"UPDATE {tableName} SET {sbCols}";
            return sQuery;
        }
    }
}