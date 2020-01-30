using ReactVR_API.Security.AccessTokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace ReactVR_UnitTesting
{
    public class JwtUnitTests
    {
        [Fact]
        public void TestLogin()
        {
            // Arrange
            var jwt = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImRhOTY2YTRhLTgxOTAtNDZjOC04OTk4LTEyMzQ2M2Q4ODgwOCIsImV4cCI6MTU4MjkzNTM5OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA3MSIsImF1ZCI6Ik15QXVkaWVuY2UifQ.Q61310iT - VI0a7lqHnJqx01KPtMBpQCI_6DahtFZwBg";
            var issuerToken = "0844AB5B0222F2E5D497BAC5FAF6CCD573E1C8BF1DF267F5B507F8EC985578D8";
            var audience = "https://localhost:7071";
            var issuer = "MyAudience";

            var tokenProvider = new AccessTokenProvider(issuerToken, audience, issuer);

            //var req = WebRequest.Create("");
            //var request = new Microsoft.AspNetCore.Http.HttpRequest();
            //req.Headers[""] = "";


            //// Act
            //tokenProvider.ValidateToken(req);
            // Assert

        }
    }
}
