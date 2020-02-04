using ReactVR_API.Security.AccessTokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ReactVR_UnitTesting
{
    public class JwtUnitTests
    {
        [Fact]
        public void TestTokenSuccessfulValidation()
        {
            // Arrange
            var key = "0844AB5B0222F2E5D497BAC5FAF6CCD573E1C8BF1DF267F5B507F8EC985578D8";
            var audience = "https://localhost:7071";
            var issuer = "MyAudience";

            var tokenCreator = new AccessTokenCreator(key, audience, issuer);
            var newToken = tokenCreator.CreateToken(Guid.NewGuid());

            // Act
            bool validated = false;

            try
            {    
                // Create the parameters
                var tokenParams = new TokenValidationParameters()
                {
                    RequireSignedTokens = true,
                    ValidAudience = audience,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

                // Validate the token
                var handler = new JwtSecurityTokenHandler();
                var result = handler.ValidateToken(newToken, tokenParams, out var securityToken);
                validated = true;
            }
            catch (Exception ex)
            {
                validated = false;
            }

            // Assert
            Assert.True(validated);
        }

        [Fact]
        public void TestTokenFailedValidation()
        {
            // Arrange
            var key = "0844AB5B0222F2E5D497BAC5FAF6CCD573E1C8BF1DF267F5B507F8EC985578D8";
            var audience = "https://localhost:7071";
            var issuer = "MyAudience";

            var tokenCreator = new AccessTokenCreator(key, audience, issuer);
            var newToken = tokenCreator.CreateToken(Guid.NewGuid());

            key = "0844AB5B0222F2E5D497BAC5FAF6CCD573E1C8BF1DF267F5B507F8EC985578D7";

            // Act
            bool validated = false;

            try
            {
                // Create the parameters
                var tokenParams = new TokenValidationParameters()
                {
                    RequireSignedTokens = true,
                    ValidAudience = audience,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

                // Validate the token
                var handler = new JwtSecurityTokenHandler();
                var result = handler.ValidateToken(newToken, tokenParams, out var securityToken);
                validated = true;
            }
            catch (Exception ex)
            {
                validated = false;
            }

            //Assert
            Assert.False(validated);
        }
    }
}
