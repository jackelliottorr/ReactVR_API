using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Core.Repositories;
using ReactVR_API.Common.Models;
using ReactVR_API.Core.Security.AccessTokens;
using ReactVR_API.Core.HelperClasses;
using System.Linq;

namespace ReactVR_API.Core.Functions
{
    public class LevelFunctions
    {
        #region Private Fields

        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AccessTokenCreator _tokenCreator;

        #endregion

        #region Constructor

        public LevelFunctions()
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

        [FunctionName("GetAllLevels")]
        public async Task<IActionResult> GetAllLevels(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Level/GetAllLevels")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(GetLevels) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                var levelRepo = new LevelRepository();
                var levels = levelRepo.GetAllLevels();

                return new OkObjectResult(levels);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //[FunctionName("CreateLevel")]
        //public static async Task<IActionResult> CreateLevel(
        // [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "level")] HttpRequest req, ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function(CreateLevel) processed a request.");

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var level = JsonConvert.DeserializeObject<Level>(requestBody);

        //    try
        //    {
        //        var levelRepo = new LevelRepository();
        //        var newId = levelRepo.CreateLevel(level);

        //        return new OkObjectResult($"Level created with id {newId}.");
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        //[FunctionName("GetLevelByLevelId")]
        //public static async Task<IActionResult> GetLevelByLevelId(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "level/{LevelId}")] HttpRequest req, ILogger log, Guid levelId)
        //{
        //    log.LogInformation("C# HTTP trigger function(GetLevelByLevelId) processed a request.");

        //    try
        //    {
        //        var levelRepo = new LevelRepository();
        //        var level = levelRepo.GetLevelByLevelId(levelId);

        //        return new OkObjectResult(level);
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        //[FunctionName("UpdateLevel")]
        //public static async Task<IActionResult> UpdateLevel(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "level")] HttpRequest req, ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function(UpdateLevel) processed a request.");

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var level = JsonConvert.DeserializeObject<Level>(requestBody);

        //    try
        //    {
        //        var levelRepo = new LevelRepository();
        //        levelRepo.UpdateLevel(level);

        //        return new OkObjectResult($"Updated {level.Name}.");
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        //[FunctionName("DeleteLevel")]
        //public static async Task<IActionResult> DeleteLevel(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "level/{LevelId}")] HttpRequest req, ILogger log, Guid levelId)
        //{
        //    log.LogInformation("C# HTTP trigger function(DeleteLevel) processed a request.");

        //    try
        //    {
        //        var levelRepo = new LevelRepository();
        //        levelRepo.DeleteLevel(levelId);

        //        return new OkObjectResult($"Deleted {levelId}");
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        #endregion
    }
}
