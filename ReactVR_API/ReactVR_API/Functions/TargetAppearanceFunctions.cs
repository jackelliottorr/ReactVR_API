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
using System.Collections.Generic;

namespace ReactVR_API.Functions
{
    public static class TargetAppearanceFunctions
    {
        [FunctionName("CreateTargetAppearance")]
        public static async Task<IActionResult> CreateTargetAppearance(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "targetAppearance")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateTargetAppearance) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var targetAppearance = JsonConvert.DeserializeObject<TargetAppearance>(requestBody);

            try
            {
                var targetAppearanceRepo = new TargetAppearanceRepository();
                var newId = targetAppearanceRepo.CreateTargetAppearance(targetAppearance);

                return new OkObjectResult($"TargetAppearance created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("CreateTargetAppearances")]
        public static async Task<IActionResult> CreateTargetAppearances(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "targetAppearances")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateTargetAppearances) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var targetAppearances = JsonConvert.DeserializeObject<IEnumerable<TargetAppearance>>(requestBody);

            try
            {
                var targetAppearanceRepo = new TargetAppearanceRepository();

                List<Guid> targetAppearanceIds = new List<Guid>();
                foreach (var targetAppearance in targetAppearances)
                {
                    targetAppearanceIds.Add(targetAppearanceRepo.CreateTargetAppearance(targetAppearance));
                }

                return new OkObjectResult($"TargetAppearances created with ids: {string.Join(",", targetAppearanceIds)}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetTargetAppearanceById")]
        public static async Task<IActionResult> GetTargetAppearanceById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targetAppearance/{TargetAppearanceId}")] HttpRequest req, ILogger log, Guid targetAppearanceId)
        {
            log.LogInformation("C# HTTP trigger function(GetTargetAppearanceById) processed a request.");

            try
            {
                var targetAppearanceRepo = new TargetAppearanceRepository();
                var targetAppearance = targetAppearanceRepo.GetTargetAppearanceById(targetAppearanceId);

                return new OkObjectResult(targetAppearance);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //[FunctionName("UpdateTargetAppearance")]
        //public static async Task<IActionResult> UpdateTargetAppearance(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "targetAppearance")] HttpRequest req, ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function(UpdateTargetAppearance) processed a request.");

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var targetAppearance = JsonConvert.DeserializeObject<TargetAppearance>(requestBody);

        //    try
        //    {
        //        var targetAppearanceRepo = new TargetAppearanceRepository();
        //        targetAppearanceRepo.UpdateTargetAppearance(targetAppearance);

        //        return new OkObjectResult($"Updated {targetAppearance.TargetAppearanceId}.");
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        [FunctionName("DeleteTargetAppearance")]
        public static async Task<IActionResult> DeleteTargetAppearance(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "targetAppearance/{TargetAppearanceId}")] HttpRequest req, ILogger log, Guid targetAppearanceId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteTargetAppearance) processed a request.");

            try
            {
                var targetAppearanceRepo = new TargetAppearanceRepository();
                targetAppearanceRepo.DeleteTargetAppearance(targetAppearanceId);

                return new OkObjectResult($"Deleted {targetAppearanceId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
