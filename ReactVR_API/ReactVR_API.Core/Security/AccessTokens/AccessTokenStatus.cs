using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Core.Security.AccessTokens
{
    public enum AccessTokenStatus
    {
        Valid,
        Expired,
        Error,
        NoToken
    }
}
