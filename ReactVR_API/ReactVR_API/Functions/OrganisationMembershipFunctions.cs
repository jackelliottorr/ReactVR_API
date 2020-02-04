using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_CORE.Models;
using ReactVR_API.Repositories;

namespace ReactVR_API.Functions
{
    public static class OrganisationMembershipFunctions
    {
        [FunctionName("CreateOrganisationMembership")]
        public static async Task<IActionResult> CreateOrganisationMembership(
       [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organisationMembership")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateOrganisationMembership) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var organisationMembership = JsonConvert.DeserializeObject<OrganisationMembership>(requestBody);

            try
            {
                var organisationMembershipRepo = new OrganisationMembershipRepository();
                var newId = organisationMembershipRepo.CreateOrganisationMembership(organisationMembership);

                return new OkObjectResult($"OrganisationMembership created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetOrganisationMembershipById")]
        public static async Task<IActionResult> GetOrganisationMembershipById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organisationMembership/{OrganisationMembershipId}")] HttpRequest req, ILogger log, Guid organisationMembershipId)
        {
            log.LogInformation("C# HTTP trigger function(GetOrganisationMembershipById) processed a request.");

            try
            {
                var organisationMembershipRepo = new OrganisationMembershipRepository();
                var organisationMembership = organisationMembershipRepo.GetOrganisationMembershipById(organisationMembershipId);

                return new OkObjectResult(organisationMembership);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateOrganisationMembership")]
        public static async Task<IActionResult> UpdateOrganisationMembership(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "organisationMembership")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateOrganisationMembership) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var organisationMembership = JsonConvert.DeserializeObject<OrganisationMembership>(requestBody);

            try
            {
                var organisationMembershipRepo = new OrganisationMembershipRepository();
                organisationMembershipRepo.UpdateOrganisationMembership(organisationMembership);

                return new OkObjectResult($"Updated organisation membership.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeleteOrganisationMembership")]
        public static async Task<IActionResult> DeleteOrganisationMembership(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "organisationMembership/{OrganisationMembershipId}")] HttpRequest req, ILogger log, Guid organisationMembershipId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteOrganisationMembership) processed a request.");

            try
            {
                var organisationMembershipRepo = new OrganisationMembershipRepository();
                organisationMembershipRepo.DeleteOrganisationMembership(organisationMembershipId);

                return new OkObjectResult($"Deleted {organisationMembershipId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
