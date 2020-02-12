using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Core.HelperClasses
{
    public static class TemporaryEnvironmentVariables
    {
        public static string GetIssuerToken()
        {
            return "4716915EDA396858D61992CB3DB830C95B4E7EC5DB50A8896658753CE9ACA3D0";
        }

        public static string GetIssuer()
        {
            return "http://localhost:7071";
        }

        public static string GetAudience()
        {
            return "MyAudience";
        }

        public static string GetDatabaseConnectionString()
        {
            return "Data Source=JACKSPC\\MSSQLSERVER2019;Initial Catalog=ReactVR_DatabaseProject;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
    }
}
