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
    public static class LevelFunctions
    {
        [FunctionName("CreateLevel")]
        public static async Task<IActionResult> CreateLevel(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "level")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateLevel) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var level = JsonConvert.DeserializeObject<Level>(requestBody);

            try
            {
                var levelRepo = new LevelRepository();
                var newId = levelRepo.CreateLevel(level);

                return new OkObjectResult($"Level created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetLevelByLevelId")]
        public static async Task<IActionResult> GetLevelByLevelId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "level/{LevelId}")] HttpRequest req, ILogger log, Guid levelId)
        {
            log.LogInformation("C# HTTP trigger function(GetLevelByLevelId) processed a request.");

            try
            {
                var levelRepo = new LevelRepository();
                var level = levelRepo.GetLevelByLevelId(levelId);

                return new OkObjectResult(level);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateLevel")]
        public static async Task<IActionResult> UpdateLevel(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "level")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateLevel) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var level = JsonConvert.DeserializeObject<Level>(requestBody);

            try
            {
                var levelRepo = new LevelRepository();
                levelRepo.UpdateLevel(level);

                return new OkObjectResult($"Updated {level.Name}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeleteLevel")]
        public static async Task<IActionResult> DeleteLevel(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "level/{LevelId}")] HttpRequest req, ILogger log, Guid levelId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteLevel) processed a request.");

            try
            {
                var levelRepo = new LevelRepository();
                levelRepo.DeleteLevel(levelId);

                return new OkObjectResult($"Deleted {levelId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
