using ReactVR_API.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Repositories
{
    class BaseRepository
    {
        protected string _connectionString = SQLConnectionSet.GetConnectionString();
    }
}
