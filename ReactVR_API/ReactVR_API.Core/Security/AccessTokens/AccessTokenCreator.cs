using Microsoft.IdentityModel.Tokens;
using ReactVR_API.Common.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactVR_API.Core.Security.AccessTokens
{
    public class AccessTokenCreator
    {
        private string _issuerToken;
        private string _audience;
        private string _issuer;

        public AccessTokenCreator(string issuerToken, string audience, string issuer)
        {
            _issuerToken = issuerToken;
            _audience = audience;
            _issuer = issuer;
        }

        /// <summary>
        /// Creates a JSON Web Token
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <returns></returns>
        public string CreateToken(Guid userAccountId)
        {
            if (userAccountId == null)
            {
                throw new ArgumentException("UserAccountId");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userAccountId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(_issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: creds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.WriteToken(token);
            return securityToken;
        }

        /// <summary>
        /// Creates a JSON Web Token
        /// </summary>
        /// <param name="userAccountId"></param>
        /// <param name="organisationId"></param>
        /// <returns></returns>
        public string CreateToken(Guid userAccountId, Guid organisationId)
        {
            if (userAccountId == null)
            {
                throw new ArgumentException("UserAccountId");
            }
            else if (organisationId == null)
            {
                throw new ArgumentException("OrganisationId");
            }

            var claims = new[]
            {
                new Claim("UserAccount", userAccountId.ToString()),
                new Claim("Organisation", organisationId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(_issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: creds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.WriteToken(token);
            return securityToken;
        }
    }
}
