using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Models;
using ReactVR_API.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReactVR_API.Functions
{
    public static class UserAccountFunctions
    {
        [FunctionName("CreateUserAccount")]
        public static async Task<IActionResult> CreateUserAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "useraccount")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateUserAccount) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var userAccount = JsonConvert.DeserializeObject<UserAccount>(requestBody);

            try
            {
                var userAccountRepo = new UserAccountRepository();
                var newId = userAccountRepo.CreateUserAccount(userAccount);

                return new OkObjectResult($"UserAccount created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetUserAccountByUserAccountId")]
        public static async Task<IActionResult> GetUserAccountByUserAccountId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "useraccount/{UserAccountId}")] HttpRequest req, ILogger log, Guid userAccountId)
        {
            log.LogInformation("C# HTTP trigger function(GetUserAccountByUserAccountId) processed a request.");

            try
            {
                var userAccountRepository = new UserAccountRepository();
                var userAccount = userAccountRepository.GetUserAccountById(userAccountId);

                if (userAccount == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(userAccount);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UpdateUserAccount")]
        public static async Task<IActionResult> UpdateUserAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "useraccount")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(UpdateUserAccount) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var userAccount = JsonConvert.DeserializeObject<UserAccount>(requestBody);

            try
            {
                var userAccountRepo = new UserAccountRepository();
                userAccountRepo.UpdateUserAccount(userAccount);

                return new OkResult();
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }     
        }

        [FunctionName("DeleteUserAccount")]
        public static async Task<IActionResult> DeleteUserAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "useraccount/{UserAccountId}")] HttpRequest req, ILogger log, Guid userAccountId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteUserAccount) processed a request.");

            try
            {
                var userAccountRepo = new UserAccountRepository();
                var result = userAccountRepo.DeleteUserAccount(userAccountId);

                return new OkObjectResult($"UserAccount({userAccountId}) deleted.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}