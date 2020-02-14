using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Common.Models;
using ReactVR_API.Core.Repositories;
using ReactVR_API.Core.Security.AccessTokens;
using ReactVR_API.Core.HelperClasses;
using System.Linq;
using System.Security.Claims;
using ReactVR_API.Common.Enums;

namespace ReactVR_API.Core.Functions
{
    public class OrganisationInviteFunctions
    {
        #region Private Fields

        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AccessTokenCreator _tokenCreator;

        #endregion

        #region Constructor

        public OrganisationInviteFunctions()
        {
            //var issuerToken = Environment.GetEnvironmentVariable("IssuerToken");
            //var audience = Environment.GetEnvironmentVariable("Audience");
            //var issuer = Environment.GetEnvironmentVariable("Issuer");

            var issuerToken = TemporaryEnvironmentVariables.GetIssuerToken();
            var audience = TemporaryEnvironmentVariables.GetAudience();
            var issuer = TemporaryEnvironmentVariables.GetIssuer();

            _tokenCreator = new AccessTokenCreator(issuerToken, audience, issuer);
            _tokenProvider = new AccessTokenProvider(issuerToken, audience, issuer);
        }

        #endregion

        #region Functions

        [FunctionName("CreateOrganisationInvite")]
        public async Task<IActionResult> CreateOrganisationInvite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "OrganisationInvite/CreateOrganisationInvite")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateOrganisationInvite) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);
                Guid organisationId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "Organisation").Value);

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var organisationInviteCreateModel = JsonConvert.DeserializeObject<OrganisationInviteCreateModel>(requestBody);

                // Make sure the user being invited is in the database/actually signed up
                var userAccountRepository = new UserAccountRepository();
                var invitee = userAccountRepository.GetUserAccountByEmailAddress(organisationInviteCreateModel.InviteeEmailAddress);

                if (invitee == null)
                {
                    return new BadRequestObjectResult("User does not exist");
                }

                var organisationInvite = new OrganisationInvite()
                {
                    OrganisationId = organisationId,
                    InvitedById = userAccountId,
                    InviteeId = invitee.UserAccountId,
                    InviteUserType = organisationInviteCreateModel.InviteUserType
                };

                var organisationInviteRepo = new OrganisationInviteRepository();
                var newId = organisationInviteRepo.CreateOrganisationInvite(organisationInvite);

                return new OkObjectResult("Invited");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetInvitesForUser")]
        public async Task<IActionResult> GetInvitesForUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OrganisationInvite/GetInvitesForUser")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(GetInvitesForUser) processed a request.");
            
            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);

                var organisationInviteRepo = new OrganisationInviteRepository();
                var invites = organisationInviteRepo.GetInvitesForUser(userAccountId);
                
                return new OkObjectResult(invites);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //[FunctionName("GetOrganisationInviteById")]
        //public static async Task<IActionResult> GetOrganisationInviteById(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organisationInvite/{OrganisationInviteId}")] HttpRequest req, ILogger log, Guid organisationInviteId)
        //{
        //    log.LogInformation("C# HTTP trigger function(GetOrganisationInviteById) processed a request.");

        //    try
        //    {
        //        var organisationInviteRepo = new OrganisationInviteRepository();
        //        var organisationInvite = organisationInviteRepo.GetOrganisationInviteById(organisationInviteId);

        //        return new OkObjectResult(organisationInvite);
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        //[FunctionName("DeleteOrganisationInvite")]
        //public static async Task<IActionResult> DeleteOrganisationInvite(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "organisationInvite/{OrganisationInviteId}")] HttpRequest req, ILogger log, Guid organisationInviteId)
        //{
        //    log.LogInformation("C# HTTP trigger function(DeleteOrganisationInvite) processed a request.");

        //    try
        //    {
        //        var organisationInviteRepo = new OrganisationInviteRepository();
        //        organisationInviteRepo.DeleteOrganisationInvite(organisationInviteId);

        //        return new OkObjectResult($"Deleted {organisationInviteId}");
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        [FunctionName("AcceptOrganisationInvite")]
        public async Task<IActionResult> AcceptOrganisationInvite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "OrganisationInvite/{OrganisationInviteId}")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(AcceptOrganisationInvite) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var organisationInvite = JsonConvert.DeserializeObject<OrganisationInvite>(requestBody);

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);

                var organisationInviteRepo = new OrganisationInviteRepository();
                organisationInvite = organisationInviteRepo.GetOrganisationInviteById(organisationInvite.OrganisationInviteId);

                if (userAccountId != organisationInvite.InviteeId)
                {
                    return new BadRequestObjectResult("User sending request is not the invitee");
                }

                var organisationMembership = new OrganisationMembership()
                {
                    OrganisationId = organisationInvite.OrganisationId,
                    UserAccountId = organisationInvite.InviteeId,
                    OrganisationInviteId = organisationInvite.OrganisationInviteId, 
                    UserType = organisationInvite.InviteUserType
                };

                var organisationMembershipRepo = new OrganisationMembershipRepository();
                if (organisationMembershipRepo.AlreadyHasAMembershipInOrganisation(userAccountId, organisationInvite.OrganisationId))
                {
                    return new BadRequestObjectResult("User already a member of this Organisation.");
                }

                organisationMembershipRepo.CreateOrganisationMembership(organisationMembership);
                organisationInviteRepo.UseOrganisationInvite(organisationInvite.OrganisationInviteId);

                // return JWT with the newly joined Organisation's Id
                var jwt = _tokenCreator.CreateToken(userAccountId, organisationInvite.OrganisationId);
                return new OkObjectResult(jwt);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        #endregion
    }
}
