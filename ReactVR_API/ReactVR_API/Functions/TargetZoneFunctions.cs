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
    public static class TargetZoneFunctions
    {
        [FunctionName("CreateTargetZone")]
        public static async Task<IActionResult> CreateTargetZone(
                 [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "targetZone")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateTargetZone) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var targetZone = JsonConvert.DeserializeObject<TargetZone>(requestBody);

            try
            {
                var targetZoneRepo = new TargetZoneRepository();
                var newId = targetZoneRepo.CreateTargetZone(targetZone);

                return new OkObjectResult($"TargetZone created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetTargetZoneById")]
        public static async Task<IActionResult> GetTargetZoneById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targetZone/{TargetZoneId}")] HttpRequest req, ILogger log, Guid targetZoneId)
        {
            log.LogInformation("C# HTTP trigger function(GetTargetZoneById) processed a request.");

            try
            {
                var targetZoneRepo = new TargetZoneRepository();
                var targetZone = targetZoneRepo.GetTargetZoneById(targetZoneId);

                return new OkObjectResult(targetZone);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateTargetZone")]
        public static async Task<IActionResult> UpdateTargetZone(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "targetZone")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateTargetZone) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var targetZone = JsonConvert.DeserializeObject<TargetZone>(requestBody);

            try
            {
                var targetZoneRepo = new TargetZoneRepository();
                targetZoneRepo.UpdateTargetZone(targetZone);

                return new OkObjectResult($"Updated {targetZone.TargetZoneId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeleteTargetZone")]
        public static async Task<IActionResult> DeleteTargetZone(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "targetZone/{TargetZoneId}")] HttpRequest req, ILogger log, Guid targetZoneId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteTargetZone) processed a request.");

            try
            {
                var targetZoneRepo = new TargetZoneRepository();
                targetZoneRepo.DeleteTargetZone(targetZoneId);

                return new OkObjectResult($"Deleted {targetZoneId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
