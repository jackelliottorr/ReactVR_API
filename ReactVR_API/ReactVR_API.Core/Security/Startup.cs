using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactVR_API.Core.Security.AccessTokens;

//[assembly: FunctionsStartup(typeof(ReactVR_API.Core.Security.Startup))]
//namespace ReactVR_API.Core.Security
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

