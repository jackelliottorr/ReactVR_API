using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactVR_API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReactVR_API.Functions
{
    public static class UserAccountFunctions
    {
        public static readonly List<UserAccount> accounts = new List<UserAccount>();

        [FunctionName("CreateUserAccount")]
        public static async Task<IActionResult> CreateUserAccount(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "useraccount")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreateUserAccount) processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var userAccountCreateModel = JsonConvert.DeserializeObject<UserAccountCreateModel>(requestBody);

            // valiadate userAccount
            // save to database table
            var userAccount = new UserAccount()
            {
                Name = userAccountCreateModel.Name,
                EmailAddress = userAccountCreateModel.EmailAddress,
                Password = userAccountCreateModel.Password
            };

            try
            {
                accounts.Add(userAccount);
                return new OkObjectResult($"Hello, {name}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        // Do not keep this in, just for learning
        [FunctionName("GetAllUserAccounts")]
        public static async Task<IActionResult> GetAllUserAccounts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "useraccount")] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(GetAllUserAccounts) processed a request.");

            return new OkObjectResult(accounts);
        }

        /// <summary>
        /// If we call api/useraccount/the id
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <param name="userAccountId"></param>
        /// <returns></returns>
        [FunctionName("GetUserAccountByUserAccountId")]
        public static async Task<IActionResult> GetUserAccountByUserAccountId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "useraccount/{UserAccountId}")] HttpRequest req,
        ILogger log, Guid userAccountId)
        {
            log.LogInformation("C# HTTP trigger function(GetUserAccountByUserAccountId) processed a request.");

            // replace with database logic
            var userAccount = accounts.FirstOrDefault(u => u.UserAccountId == userAccountId);
            if (userAccount == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(userAccount);
        }

        [FunctionName("UpdateUserAccount")]
        public static async Task<IActionResult> UpdateUserAccount(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "useraccount/{UserAccountId}")] HttpRequest req,
        ILogger log, Guid userAccountId)
        {
            log.LogInformation("C# HTTP trigger function(UpdateUserAccount) processed a request.");

            // replace with database logic
            var userAccount = accounts.FirstOrDefault(u => u.UserAccountId == userAccountId);
            if (userAccount == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var userAccountUpdateModel = JsonConvert.DeserializeObject<UserAccountUpdateModel>(requestBody);
            
            //validate the update model
            // if validated, update the original
            // save to db
            userAccount.Name = userAccountUpdateModel.Name;
            userAccount.EmailAddress = userAccountUpdateModel.EmailAddress;
            userAccount.Password = userAccountUpdateModel.Password;

            return new OkResult();
        }

        [FunctionName("DeleteUserAccount")]
        public static async Task<IActionResult> DeleteUserAccount(
                [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "useraccount/{UserAccountId}")] HttpRequest req,
                ILogger log, Guid userAccountId)
        {
            log.LogInformation("C# HTTP trigger function(DeleteUserAccount) processed a request.");

            // replace with database logic
            var userAccount = accounts.FirstOrDefault(u => u.UserAccountId == userAccountId);
            if (userAccount == null)
            {
                return new NotFoundResult();
            }

            accounts.Remove(userAccount);
            // db layer instead ofc

            return new OkResult();
        }
    }
}