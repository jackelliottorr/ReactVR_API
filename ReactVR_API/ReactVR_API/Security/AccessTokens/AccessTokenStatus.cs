using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Security.AccessTokens
{
    public enum AccessTokenStatus
    {
        Valid,
        Expired,
        Error,
        NoToken
    }
}
