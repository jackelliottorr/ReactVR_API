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
    public class LevelConfigurationFunctions
    {
        #region Private Fields

        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AccessTokenCreator _tokenCreator;

        #endregion

        #region Constructor

        public LevelConfigurationFunctions()
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

        [FunctionName("GetLevelConfigurationsByLevelId")]
        public async Task<IActionResult> GetLevelConfigurationsByLevelId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LevelConfiguration/{LevelId}")] HttpRequest req, ILogger log, Guid levelId)
        {
            log.LogInformation("C# HTTP trigger function(GetLevelConfigurationsByLevelId) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);

                // possibly can speed this up/less calls by using advanced Dapper features
                var levelConfigurationRepo = new LevelConfigurationRepository();
                var targetZoneRepo = new TargetZoneRepository();
                var targetRepo = new TargetRepository();

                var levelConfigurations = levelConfigurationRepo.GetLevelConfigurationsByLevelId(levelId);
                foreach (var levelConfigurationViewModel in levelConfigurations)
                {
                    levelConfigurationViewModel.TargetZone = targetZoneRepo.GetTargetZoneByLevelConfigurationId(levelConfigurationViewModel.LevelConfigurationId);
                    levelConfigurationViewModel.Targets = targetRepo.GetTargetsByLevelConfigurationId(levelConfigurationViewModel.LevelConfigurationId);
                }

                return new OkObjectResult(levelConfigurations);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetLevelConfigurationsByCreatedById")]
        public async Task<IActionResult> GetLevelConfigurationsByCreatedById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LevelConfiguration/GetLevelConfigurationsByCreatedById")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(GetLevelConfigurationsByCreatedById) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);

                // possibly can speed this up/less calls by using advanced Dapper features
                var levelConfigurationRepo = new LevelConfigurationRepository();
                var targetZoneRepo = new TargetZoneRepository();
                var targetRepo = new TargetRepository();

                var levelConfigurations = levelConfigurationRepo.GetLevelConfigurationsByCreatedById(userAccountId);
                foreach (var levelConfigurationViewModel in levelConfigurations)
                {
                    levelConfigurationViewModel.TargetZone = targetZoneRepo.GetTargetZoneByLevelConfigurationId(levelConfigurationViewModel.LevelConfigurationId);
                    levelConfigurationViewModel.Targets = targetRepo.GetTargetsByLevelConfigurationId(levelConfigurationViewModel.LevelConfigurationId);
                }

                return new OkObjectResult(levelConfigurations);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("CreateLevelConfiguration")]
        public async Task<IActionResult> CreateLevelConfiguration(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "levelConfiguration")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateLevelConfiguration) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var levelConfiguration = JsonConvert.DeserializeObject<LevelConfiguration>(requestBody);

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                var levelConfigurationRepo = new LevelConfigurationRepository();
                var newId = levelConfigurationRepo.CreateLevelConfiguration(levelConfiguration);

                return new OkObjectResult($"LevelConfiguration created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateLevelConfiguration")]
        public async Task<IActionResult> UpdateLevelConfiguration(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "levelConfiguration")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateLevelConfiguration) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var levelConfiguration = JsonConvert.DeserializeObject<LevelConfiguration>(requestBody);

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

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
        public async Task<IActionResult> DeleteLevelConfiguration(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "levelConfiguration/{LevelConfigurationId}")] HttpRequest req, ILogger log, Guid levelConfigurationId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteLevelConfiguration) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                var levelConfigurationRepo = new LevelConfigurationRepository();
                levelConfigurationRepo.DeleteLevelConfiguration(levelConfigurationId);

                return new OkObjectResult($"Deleted {levelConfigurationId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        #endregion
    }
}
