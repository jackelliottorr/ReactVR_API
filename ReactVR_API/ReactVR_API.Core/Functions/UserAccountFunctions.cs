using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Core.HelperClasses;
using ReactVR_API.Common.Models;
using ReactVR_API.Core.Repositories;
using ReactVR_API.Core.Security.AccessTokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using ReactVR_API.Core.Security.Login;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace ReactVR_API.Core.Functions
{
    public class UserAccountFunctions
    {

        private readonly IAccessTokenProvider _tokenProvider;
        private readonly AccessTokenCreator _tokenCreator;

        // for DI, pass tokenProvider to constructor instead of instantiating here
        //var issuerToken = TemporaryEnvironmentVariables.GetIssuerToken();
        //var audience = TemporaryEnvironmentVariables.GetAudience();
        //var issuer = TemporaryEnvironmentVariables.GetIssuer();
        public UserAccountFunctions()
        {
            //var issuerToken = TemporaryEnvironmentVariables.GetIssuerToken();
            //var audience = TemporaryEnvironmentVariables.GetAudience();
            //var issuer = TemporaryEnvironmentVariables.GetIssuer();

            var issuerToken = Environment.GetEnvironmentVariable("IssuerToken");
            var audience = Environment.GetEnvironmentVariable("Audience");
            var issuer = Environment.GetEnvironmentVariable("Issuer");

            _tokenCreator = new AccessTokenCreator(issuerToken, audience, issuer);
            _tokenProvider = new AccessTokenProvider(issuerToken, audience, issuer);
        }

        #region Functions

        [FunctionName("TestConnection")]
        public async Task<IActionResult> TestConnection(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UserAccount/TestConnection")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(TestConnection) processed a request.");

            try
            {
                return new OkObjectResult($"Successful connection at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

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

        [FunctionName("ValidateAccessToken")]
        public async Task<IActionResult> ValidateAccessToken(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UserAccount/ValidateAccessToken")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(ValidateAccessToken) processed a request.");

            var accessTokenResult = _tokenProvider.ValidateToken(req);

            if (accessTokenResult.Status == AccessTokenStatus.Valid)
            {
                try
                {
                    var userAccountId = accessTokenResult.Principal.Claims.First(c => c.Type == "UserAccount").Value;
                    log.LogInformation($"JWT validated for UserAccount: {userAccountId}.");

                    // return updated JWT with UserAccountId
                    var jwt = _tokenCreator.CreateToken(new Guid(userAccountId));
                    return new OkObjectResult(jwt);
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

                    return new OkObjectResult("Updated");
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

        [FunctionName("ChangeEmailAddress")]
        public async Task<IActionResult> ChangeEmailAddress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UserAccount/ChangeEmailAddress")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(ChangeEmailAddress) processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userAccountUpdateModel = JsonConvert.DeserializeObject<UserAccountUpdateModel>(requestBody);
                
                var loginManager = new LoginManager();
                var loginResult = loginManager.AttemptLogin(userAccountUpdateModel.EmailAddress, userAccountUpdateModel.Password);

                if (loginResult.Status == LoginStatus.Success)
                {
                    // Make sure the email address is not already in use
                    if (loginManager.AccountExists(userAccountUpdateModel.NewEmailAddress))
                    {
                        return new BadRequestObjectResult("Email Address is already in use.");
                    }
                    else
                    {
                        loginResult.UserAccount.EmailAddress = userAccountUpdateModel.NewEmailAddress;
                    }

                    // Update the UserAccount with the new value
                    var userAccountRepo = new UserAccountRepository();
                    userAccountRepo.UpdateUserAccount(loginResult.UserAccount);

                    return new OkObjectResult($"Email Address updated to {userAccountUpdateModel.NewEmailAddress}).");
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

        [FunctionName("ChangePassword")]
        public async Task<IActionResult> ChangePassword(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UserAccount/ChangePassword")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(ChangePassword) processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var userAccountUpdateModel = JsonConvert.DeserializeObject<UserAccountUpdateModel>(requestBody);

                var loginManager = new LoginManager();
                var loginResult = loginManager.AttemptLogin(userAccountUpdateModel.EmailAddress, userAccountUpdateModel.Password);

                if (loginResult.Status == LoginStatus.Success)
                {
                    var salt = PasswordManager.GenerateSalt();
                    var hash = PasswordManager.HashPassword(userAccountUpdateModel.NewPassword, salt);

                    loginResult.UserAccount.Salt = salt;
                    loginResult.UserAccount.Hash = hash;

                    var userAccountRepo = new UserAccountRepository();
                    userAccountRepo.UpdateUserAccount(loginResult.UserAccount);

                    return new OkObjectResult($"Password updated.");
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