using ReactVR_API.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Core.Repositories
{
    class BaseRepository
    {
        protected string _connectionString = SQLConnectionSet.GetConnectionString();
    }
}
