using System;
using Microsoft.Extensions.DependencyInjection;
using ReactVR_API.Security.AccessTokens;

//[assembly: FunctionsStartup(typeof(ReactVR_API.Security.Startup))]
//namespace ReactVR_API.Security
//{
//    /// <summary>
//    /// Runs when the Azure Functions host starts.
//    /// </summary>
//    public class Startup : FunctionsStartup
//    {
//        public override void Configure(IFunctionsHostBuilder builder)
//        {
//            // Get the configuration files for the OAuth token issuer
//            var issuerToken = Environment.GetEnvironmentVariable("IssuerToken");
//            var audience = Environment.GetEnvironmentVariable("Audience");
//            var issuer = Environment.GetEnvironmentVariable("Issuer");

//            // Register the access token provider as a singleton
//            builder.Services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>(s => new AccessTokenProvider(issuerToken, audience, issuer));
//        }
//    }
//}
