using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Core.HelperClasses
{
    public static class SQLConnectionSet
    {
        public static string GetConnectionString()
        {
            var connectionString = TemporaryEnvironmentVariables.GetDatabaseConnectionString();
            //var connectionString = Environment.GetEnvironmentVariable("ReactVR_DatabaseConnectionString", EnvironmentVariableTarget.Process);

            return connectionString;
        }
    }
}
