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
using ReactVR_API.Core.Security.AccessTokens;
using ReactVR_API.Core.HelperClasses;
using System.Linq;

namespace ReactVR_API.Core.Functions
{
    public class ScoreboardFunctions
    {
        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AccessTokenCreator _tokenCreator;

        public ScoreboardFunctions()
        {
            var issuerToken = Environment.GetEnvironmentVariable("IssuerToken");
            var audience = Environment.GetEnvironmentVariable("Audience");
            var issuer = Environment.GetEnvironmentVariable("Issuer");

            _tokenCreator = new AccessTokenCreator(issuerToken, audience, issuer);
            _tokenProvider = new AccessTokenProvider(issuerToken, audience, issuer);
        }
        
        #region Functions

        [FunctionName("CreateScoreboardEntry")]
        public async Task<IActionResult> CreateScoreboardEntry(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Scoreboard/CreateScoreboardEntry")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateScoreboardEntry) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var scoreboardCreateModel = JsonConvert.DeserializeObject<ScoreboardCreateModel>(requestBody);

                var scoreboard = new Scoreboard()
                {
                    UserAccountId = userAccountId,
                    LevelConfigurationId = scoreboardCreateModel.LevelConfigurationId,
                    Score = scoreboardCreateModel.Score
                };

                var scoreboardRepo = new ScoreboardRepository();
                var newId = scoreboardRepo.CreateScoreboard(scoreboard);

                return new OkObjectResult($"Score logged.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetScoreboardForLevelConfiguration")]
        public async Task<IActionResult> GetScoreboardForLevelConfiguration(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Scoreboard/{LevelConfigurationId}")] HttpRequest req, ILogger log, Guid levelConfigurationId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteScoreboard) processed a request.");

            try
            {
                var accessTokenResult = _tokenProvider.ValidateToken(req);
                if (accessTokenResult.Status != AccessTokenStatus.Valid)
                {
                    return new UnauthorizedResult();
                }

                Guid userAccountId = new Guid(accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value);

                var scoreboardRepo = new ScoreboardRepository();
                var scores = scoreboardRepo.GetScoreboardForLevelConfiguration(levelConfigurationId);

                return new OkObjectResult(scores);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        #endregion
    }
}
