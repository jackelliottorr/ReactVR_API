using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.HelperClasses;
using ReactVR_CORE.Models;
using ReactVR_API.Repositories;
using ReactVR_API.Security.AccessTokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using ReactVR_API.Security.Login;

namespace ReactVR_API.Functions
{
    public class UserAccountFunctions
    {
        #region Private Fields

        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AccessTokenCreator _tokenCreator;

        #endregion

        #region Constructor

        // for DI, pass tokenProvider to constructor instead of instantiating here
        public UserAccountFunctions()
        {
            var issuerToken = Environment.GetEnvironmentVariable("IssuerToken");
            var audience = Environment.GetEnvironmentVariable("Audience");
            var issuer = Environment.GetEnvironmentVariable("Issuer");

            _tokenCreator = new AccessTokenCreator(issuerToken, audience, issuer);
            _tokenProvider = new AccessTokenProvider(issuerToken, audience, issuer);
        }

        #endregion

        #region Functions

        [FunctionName("CreateUserAccount")]
        public async Task<IActionResult> CreateUserAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UserAccount/CreateUserAccount")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateUserAccount) processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userAccountCreateModel = JsonConvert.DeserializeObject<UserAccountCreateModel>(requestBody);

                // first check if username already exists
                var loginManager = new LoginManager();

                if (loginManager.AccountExists(userAccountCreateModel.EmailAddress))
                {
                    return new BadRequestObjectResult("An account with this email address already exists.");
                }

                // get salt and hash the password
                var salt = PasswordManager.GenerateSalt();
                var hash = PasswordManager.HashPassword(userAccountCreateModel.Password, salt);

                // instantiate new UserAccount with salt and hashed password
                var userAccount = new UserAccount()
                {
                    Name = userAccountCreateModel.Name,
                    EmailAddress = userAccountCreateModel.EmailAddress,
                    Salt = salt,
                    Hash = hash
                };

                // save useraccount to database
                var userAccountRepo = new UserAccountRepository();
                var newId = userAccountRepo.CreateUserAccount(userAccount);

                // return JWT with UserAccountId
                var jwt = _tokenCreator.CreateToken(newId);
                return new OkObjectResult(jwt);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UserAccount/Login")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(Login) processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userAccountCreateModel = JsonConvert.DeserializeObject<UserAccountCreateModel>(requestBody);

                var loginManager = new LoginManager();
                var loginResult = loginManager.AttemptLogin(userAccountCreateModel.EmailAddress, userAccountCreateModel.Password);

                if (loginResult.Status == LoginStatus.Success)
                {
                    var userAccountRepo = new UserAccountRepository();
                    userAccountRepo.DeleteUserAccount(loginResult.UserAccount.UserAccountId);

                    // return JWT with UserAccountId
                    var jwt = _tokenCreator.CreateToken(loginResult.UserAccount.UserAccountId);
                    return new OkObjectResult(jwt);
                }
                else
                {
                    // maybe change this so there's only 1 fail condition instead of having Error & Failure
                    return new BadRequestObjectResult(loginResult.FailureReason);
                }
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //[FunctionName("GetUserAccountByUserAccountId")]
        //public async Task<IActionResult> GetUserAccountByUserAccountId(
        //[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "useraccount/{UserAccountId}")] HttpRequest req, ILogger log, Guid userAccountId)
        //{
        //    log.LogInformation("C# HTTP trigger function(GetUserAccountByUserAccountId) processed a request.");

        //    try
        //    {
        //        var userAccountRepository = new UserAccountRepository();
        //        var userAccount = userAccountRepository.GetUserAccountById(userAccountId);

        //        if (userAccount == null)
        //        {
        //            return new NotFoundResult();
        //        }

        //        return new OkObjectResult(userAccount);
        //    }
        //    catch (Exception exception)
        //    {
        //        return new BadRequestObjectResult(exception.Message);
        //    }
        //}

        /// <summary>
        /// haven't had to change attributes/parameters since we inject the tokenprovider
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("UpdateUserAccount")]
        public async Task<IActionResult> UpdateUserAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "UserAccount/UpdateUserAccount")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateUserAccount) processed a request.");

            var accessTokenResult = _tokenProvider.ValidateToken(req);

            if (accessTokenResult.Status == AccessTokenStatus.Valid)
            {
                try
                {
                    var userAccountId = accessTokenResult.Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    log.LogInformation($"JWT validated for UserAccount: {userAccountId}.");

                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    var updateModel = JsonConvert.DeserializeObject<UserAccountUpdateModel>(requestBody);

                    var userAccountRepo = new UserAccountRepository();
                    var userAccount = userAccountRepo.GetUserAccountById(new Guid(userAccountId));

                    ModelUpdater.CopyProperties(updateModel, userAccount);

                    bool updated = userAccountRepo.UpdateUserAccount(userAccount);

                    return new OkResult();
                }
                catch (Exception exception)
                {
                    return new BadRequestObjectResult(exception.Message);
                }
            }
            else
            {
                return new UnauthorizedResult();
            }
        }

        [FunctionName("DeleteUserAccount")]
        public async Task<IActionResult> DeleteUserAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "UserAccount/DeleteUserAccount")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(DeleteUserAccount) processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userAccountCreateModel = JsonConvert.DeserializeObject<UserAccountCreateModel>(requestBody);
                
                var loginManager = new LoginManager();
                var loginResult = loginManager.AttemptLogin(userAccountCreateModel.EmailAddress, userAccountCreateModel.Password);

                if (loginResult.Status == LoginStatus.Success)
                {
                    var userAccountRepo = new UserAccountRepository();
                    userAccountRepo.DeleteUserAccount(loginResult.UserAccount.UserAccountId);

                    return new OkObjectResult($"User {loginResult.UserAccount.EmailAddress}) has been deleted.");
                }
                else
                {
                    // maybe change this so there's only 1 fail condition instead of having Error & Failure
                    return new BadRequestObjectResult(loginResult.FailureReason);
                }
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        #endregion
    }
}