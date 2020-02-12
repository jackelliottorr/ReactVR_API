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

namespace ReactVR_API.Core.Functions
{
    public static class OrganisationInviteFunctions
    {
        [FunctionName("CreateOrganisationInvite")]
        public static async Task<IActionResult> CreateOrganisationInvite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organisationInvite")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateOrganisationInvite) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var organisationInvite = JsonConvert.DeserializeObject<OrganisationInvite>(requestBody);

            try
            {
                var organisationInviteRepo = new OrganisationInviteRepository();
                var newId = organisationInviteRepo.CreateOrganisationInvite(organisationInvite);

                return new OkObjectResult($"OrganisationInvite created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetOrganisationInviteById")]
        public static async Task<IActionResult> GetOrganisationInviteById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organisationInvite/{OrganisationInviteId}")] HttpRequest req, ILogger log, Guid organisationInviteId)
        {
            log.LogInformation("C# HTTP trigger function(GetOrganisationInviteById) processed a request.");

            try
            {
                var organisationInviteRepo = new OrganisationInviteRepository();
                var organisationInvite = organisationInviteRepo.GetOrganisationInviteById(organisationInviteId);

                return new OkObjectResult(organisationInvite);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeleteOrganisationInvite")]
        public static async Task<IActionResult> DeleteOrganisationInvite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "organisationInvite/{OrganisationInviteId}")] HttpRequest req, ILogger log, Guid organisationInviteId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteOrganisationInvite) processed a request.");

            try
            {
                var organisationInviteRepo = new OrganisationInviteRepository();
                organisationInviteRepo.DeleteOrganisationInvite(organisationInviteId);

                return new OkObjectResult($"Deleted {organisationInviteId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UseOrganisationInvite")]
        public static async Task<IActionResult> UseOrganisationInvite(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organisationInvite/{OrganisationInviteId}")] HttpRequest req, ILogger log, Guid organisationInviteId)
        {
            log.LogInformation("C# HTTP trigger function(UseOrganisationInvite) processed a request.");

            try
            {
                var organisationInviteRepo = new OrganisationInviteRepository();
                organisationInviteRepo.UseOrganisationInvite(organisationInviteId);

                // now we have to actually add a row to organisation membership

                return new OkObjectResult($"Used Organisation Invite: {organisationInviteId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
