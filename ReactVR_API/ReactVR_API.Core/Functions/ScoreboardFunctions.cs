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
    public static class ScoreboardFunctions
    {
        [FunctionName("CreateScoreboard")]
        public static async Task<IActionResult> CreateScoreboard(
  [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "scoreboard")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateScoreboard) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var scoreboard = JsonConvert.DeserializeObject<Scoreboard>(requestBody);

            try
            {
                var scoreboardRepo = new ScoreboardRepository();
                var newId = scoreboardRepo.CreateScoreboard(scoreboard);

                return new OkObjectResult($"Scoreboard created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetScoreboardByScoreboardId")]
        public static async Task<IActionResult> GetScoreboardByScoreboardId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "scoreboard/{ScoreboardId}")] HttpRequest req, ILogger log, Guid scoreboardId)
        {
            log.LogInformation("C# HTTP trigger function(GetScoreboardByScoreboardId) processed a request.");

            try
            {
                var scoreboardRepo = new ScoreboardRepository();
                var scoreboard = scoreboardRepo.GetScoreboardById(scoreboardId);

                return new OkObjectResult(scoreboard);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //[FunctionName("UpdateScoreboard")]
        //public static async Task<IActionResult> UpdateScoreboard(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "scoreboard")] HttpRequest req, ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function(UpdateScoreboard) processed a request.");

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var scoreboard = JsonConvert.DeserializeObject<Scoreboard>(requestBody);

        //    try
        //    {
        //        var scoreboardRepo = new ScoreboardRepository();
        //        scoreboardRepo.UpdateScoreboard(scoreboard);

        //        return new OkObjectResult($"Updated {scoreboard.Name}.");
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        [FunctionName("DeleteScoreboard")]
        public static async Task<IActionResult> DeleteScoreboard(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "scoreboard/{ScoreboardId}")] HttpRequest req, ILogger log, Guid scoreboardId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteScoreboard) processed a request.");

            try
            {
                var scoreboardRepo = new ScoreboardRepository();
                scoreboardRepo.DeleteScoreboard(scoreboardId);

                return new OkObjectResult($"Deleted {scoreboardId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
