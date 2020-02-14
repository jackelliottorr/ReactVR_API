using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Common.Enums;
using ReactVR_API.Common.Models;
using ReactVR_API.Core.Repositories;
using ReactVR_API.Core.Security.AccessTokens;
using ReactVR_API.Core.HelperClasses;
using System.Linq;
using System.Security.Claims;
using ReactVR_API.Core.Security.Login;

namespace ReactVR_API.Core.Functions
{
     public class OrganisationFunctions
    {
        #region Private Fields

        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AccessTokenCreator _tokenCreator;

        #endregion

        #region Constructor

        public OrganisationFunctions()
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

        [FunctionName("CreateOrganisation")]
        public async Task<IActionResult> CreateOrganisation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Organisation/CreateOrganisation")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateOrganisation) processed a request.");

            var accessTokenResult = _tokenProvider.ValidateToken(req);

            if (accessTokenResult.Status == AccessTokenStatus.Valid)
            {
                try
                {
                    Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);
                    log.LogInformation($"JWT validated for UserAccount: {userAccountId}.");

                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    var organisationCreateModel = JsonConvert.DeserializeObject<OrganisationCreateModel>(requestBody);

                    var organisation = new Organisation()
                    {
                        OrganisationName = organisationCreateModel.OrganisationName,
                        CreatedById = userAccountId
                    };

                    var organisationRepo = new OrganisationRepository();
                    var organisationId = organisationRepo.CreateOrganisation(organisation);

                    var organisationMembership = new OrganisationMembership()
                    {
                        OrganisationId = organisationId,
                        UserAccountId = userAccountId,
                        UserType = UserType.OrganisationOwner,
                        OrganisationInviteId = null
                    };

                    // store the OrganisationMembership
                    var organisationMembershipRepo = new OrganisationMembershipRepository();
                    organisationMembershipRepo.CreateOrganisationMembership(organisationMembership);

                    // create JWT with the OrganisationId as 
                    var jwt = _tokenCreator.CreateToken(userAccountId, organisationId);
                    return new OkObjectResult(jwt);
                }
                catch (Exception exception)
                {
                    return new BadRequestObjectResult(exception.Message);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [FunctionName("GetOrganisationsForUser")]
        public async Task<IActionResult> GetOrganisationsForUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Organisation/GetOrganisationsForUser")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(GetOrganisationsForUser) processed a request.");

            var accessTokenResult = _tokenProvider.ValidateToken(req);

            if (accessTokenResult.Status == AccessTokenStatus.Valid)
            {
                try
                {
                    Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);
                    log.LogInformation($"JWT validated for UserAccount: {userAccountId}.");

                    // we might not need OrganisationMembershipFunctions??
                    var organisationRepository = new OrganisationRepository();
                    var organisations = organisationRepository.GetOrganisationsForUser(userAccountId);

                    return new OkObjectResult(organisations);
                }
                catch (Exception exception)
                {
                    return new BadRequestObjectResult(exception.Message);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        /// <summary>
        /// Client will send Id of User who is switching their active Organisation
        /// They will also send the Id of the Organisation being switched to
        /// Server can then return a new JWT with the OrganisationId of the target Organisation
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("ChangeActiveOrganisation")]
        public async Task<IActionResult> ChangeActiveOrganisation([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Organisation/ChangeActiveOrganisation")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(ChangeActiveOrganisation) processed a request.");

            try
            {
                // Validate JWT
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                // not sure if I should check if this organisation exists first?
                var targetOrganisation = JsonConvert.DeserializeObject<Organisation>(requestBody);

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);
                Guid organisationId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "Organisation").Value);

                // Make sure this user is a member of the organisation
                var organisationMembershipRepository = new OrganisationMembershipRepository();
                var organisationMembership = organisationMembershipRepository.GetOrganisationMembership(userAccountId, organisationId);

                if (organisationMembership != null)
                {
                    // return JWT with UserAccountId
                    var jwt = _tokenCreator.CreateToken(userAccountId, targetOrganisation.OrganisationId);
                    return new OkObjectResult(jwt);
                }
                else
                {
                    return new ConflictObjectResult("User is not a member of this Organisation. Please contact support.");
                }
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateOrganisation")]
        public async Task<IActionResult> UpdateOrganisation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "organisation")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateOrganisation) processed a request.");

            var accessTokenResult = _tokenProvider.ValidateToken(req);

            if (accessTokenResult.Status == AccessTokenStatus.Valid)
            {
                try
                {
                    Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);
                    Guid organisationId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "Organisation").Value);

                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    var organisationUpdateModel = JsonConvert.DeserializeObject<OrganisationCreateModel>(requestBody);

                    var organisationRepo = new OrganisationRepository();
                    var organisation = organisationRepo.GetOrganisationById(organisationId);

                    ModelUpdater.CopyProperties(organisationUpdateModel, organisation);

                    bool updated = organisationRepo.UpdateOrganisation(organisation);

                    return new OkObjectResult("Updated");
                }
                catch (Exception exception)
                {
                    return new BadRequestObjectResult(exception.Message);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        /// <summary>
        /// Function for deleting an Organisation
        /// First of all, user must provide a valid JWT which hasn't been tampered with - containing UserAccountId & OrganisationId
        /// Secondly, since this is an administrative action - username & password must also be provided 
        /// Thirdly, once validated & logged in - verify that the user is in fact the organisation owner
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("DeleteOrganisation")]
        public async Task<IActionResult> DeleteOrganisation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Organisation/DeleteOrganisation")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(DeleteOrganisation) processed a request.");

            try
            {
                // Validate JWT
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userAccountCreateModel = JsonConvert.DeserializeObject<UserAccountCreateModel>(requestBody);

                // Validate Email/Password
                var loginManager = new LoginManager();
                var loginResult = loginManager.AttemptLogin(userAccountCreateModel.EmailAddress, userAccountCreateModel.Password);
                if (loginResult.Status != LoginStatus.Success)
                {
                    return new BadRequestObjectResult(loginResult.FailureReason);
                }

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);
                Guid organisationId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "Organisation").Value);

                // Make sure this UserAccount is the Organisation Owner
                var organisationMembershipRepository = new OrganisationMembershipRepository();
                var organisationMembership = organisationMembershipRepository.GetOrganisationMembership(userAccountId, organisationId);

                if (organisationMembership.UserType == UserType.OrganisationOwner)
                {
                    var organisationRepo = new OrganisationRepository();
                    bool deleted = organisationRepo.DeleteOrganisation(organisationId);

                    return new OkObjectResult(deleted);
                }
                else
                {
                    return new UnauthorizedResult();
                }
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        #endregion
    }
}
