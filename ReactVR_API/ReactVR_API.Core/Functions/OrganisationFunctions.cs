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
                    Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
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
                        UserTypeId = UserType.OrganisationOwner,
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

        [FunctionName("UpdateOrganisation")]
        public async Task<IActionResult> UpdateOrganisation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "organisation")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateOrganisation) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var organisation = JsonConvert.DeserializeObject<Organisation>(requestBody);

            try
            {
                var organisationRepo = new OrganisationRepository();
                organisationRepo.UpdateOrganisation(organisation);

                return new OkObjectResult($"Updated {organisation.OrganisationName}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeleteOrganisation")]
        public async Task<IActionResult> DeleteOrganisation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "organisation/{OrganisationId}")] HttpRequest req, ILogger log, Guid organisationId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteOrganisation) processed a request.");

            try
            {
                var organisationRepo = new OrganisationRepository();
                organisationRepo.DeleteOrganisation(organisationId);

                return new OkObjectResult($"Deleted {organisationId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        #endregion
    }
}
