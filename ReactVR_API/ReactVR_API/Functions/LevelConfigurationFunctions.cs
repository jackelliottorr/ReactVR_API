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
    public static class LevelConfigurationFunctions
    {
        [FunctionName("CreateLevelConfiguration")]
        public static async Task<IActionResult> CreateLevelConfiguration(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "levelConfiguration")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateLevelConfiguration) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var levelConfiguration = JsonConvert.DeserializeObject<LevelConfiguration>(requestBody);

            try
            {
                var levelConfigurationRepo = new LevelConfigurationRepository();
                var newId = levelConfigurationRepo.CreateLevelConfiguration(levelConfiguration);

                return new OkObjectResult($"LevelConfiguration created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetLevelConfigurationById")]
        public static async Task<IActionResult> GetLevelConfigurationById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "levelConfiguration/{LevelConfigurationId}")] HttpRequest req, ILogger log, Guid levelConfigurationId)
        {
            log.LogInformation("C# HTTP trigger function(GetLevelConfigurationById) processed a request.");

            try
            {
                var levelConfigurationRepo = new LevelConfigurationRepository();
                var levelConfiguration = levelConfigurationRepo.GetLevelConfigurationById(levelConfigurationId);

                return new OkObjectResult(levelConfiguration);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateLevelConfiguration")]
        public static async Task<IActionResult> UpdateLevelConfiguration(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "levelConfiguration")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateLevelConfiguration) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var levelConfiguration = JsonConvert.DeserializeObject<LevelConfiguration>(requestBody);

            try
            {
                var levelConfigurationRepo = new LevelConfigurationRepository();
                levelConfigurationRepo.UpdateLevelConfiguration(levelConfiguration);

                return new OkObjectResult($"Updated {levelConfiguration.Name}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeleteLevelConfiguration")]
        public static async Task<IActionResult> DeleteLevelConfiguration(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "levelConfiguration/{LevelConfigurationId}")] HttpRequest req, ILogger log, Guid levelConfigurationId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteLevelConfiguration) processed a request.");

            try
            {
                var levelConfigurationRepo = new LevelConfigurationRepository();
                levelConfigurationRepo.DeleteLevelConfiguration(levelConfigurationId);

                return new OkObjectResult($"Deleted {levelConfigurationId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
