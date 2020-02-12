using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Core.Security.AccessTokens
{
    public interface IAccessTokenProvider
    {
        AccessTokenResult ValidateToken(HttpRequest request);
    }
}
