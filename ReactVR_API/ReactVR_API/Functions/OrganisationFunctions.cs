using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Models;
using ReactVR_API.Repositories;

namespace ReactVR_API.Functions
{
    public static class OrganisationFunctions
    {
        [FunctionName("CreateOrganisation")]
        public static async Task<IActionResult> CreateOrganisation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organisation")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateOrganisation) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var organisation = JsonConvert.DeserializeObject<Organisation>(requestBody);

            try
            {
                var organisationRepo = new OrganisationRepository();
                var newId = organisationRepo.CreateOrganisation(organisation);

                return new OkObjectResult($"Organisation created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetOrganisationByOrganisationId")]
        public static async Task<IActionResult> GetOrganisationByOrganisationId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organisation/{OrganisationId}")] HttpRequest req, ILogger log, Guid organisationId)
        {
            log.LogInformation("C# HTTP trigger function(GetOrganisationByOrganisationId) processed a request.");

            try
            {
                var organisationRepo = new OrganisationRepository();
                var organisation = organisationRepo.GetOrganisationByOrganisationId(organisationId);

                return new OkObjectResult(organisation);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
