using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Repositories;
using System.Collections.Generic;
using ReactVR_CORE.Models;

namespace ReactVR_API.Functions
{
    public static class TargetFunctions
    {
        [FunctionName("CreateTarget")]
        public static async Task<IActionResult> CreateTarget(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "target")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateTarget) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var target = JsonConvert.DeserializeObject<Target>(requestBody);

            try
            {
                var targetRepo = new TargetRepository();
                var newId = targetRepo.CreateTarget(target);

                return new OkObjectResult($"Target created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("CreateTargets")]
        public static async Task<IActionResult> CreateTargets(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "targets")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateTargets) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var targets = JsonConvert.DeserializeObject<IEnumerable<Target>>(requestBody);

            try
            {
                var targetRepo = new TargetRepository();

                List<Guid> targetIds = new List<Guid>();
                foreach (var target in targets)
                {
                    targetIds.Add(targetRepo.CreateTarget(target));
                }

                return new OkObjectResult($"Targets created with ids: {string.Join(",", targetIds)}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetTargetById")]
        public static async Task<IActionResult> GetTargetById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "target/{TargetId}")] HttpRequest req, ILogger log, Guid targetId)
        {
            log.LogInformation("C# HTTP trigger function(GetTargetById) processed a request.");

            try
            {
                var targetRepo = new TargetRepository();
                var target = targetRepo.GetTargetById(targetId);

                return new OkObjectResult(target);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateTarget")]
        public static async Task<IActionResult> UpdateTarget(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "target")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateTarget) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var target = JsonConvert.DeserializeObject<Target>(requestBody);

            try
            {
                var targetRepo = new TargetRepository();
                targetRepo.UpdateTarget(target);

                return new OkObjectResult($"Updated {target.TargetId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeleteTarget")]
        public static async Task<IActionResult> DeleteTarget(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "target/{TargetId}")] HttpRequest req, ILogger log, Guid targetId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteTarget) processed a request.");

            try
            {
                var targetRepo = new TargetRepository();
                targetRepo.DeleteTarget(targetId);

                return new OkObjectResult($"Deleted {targetId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
