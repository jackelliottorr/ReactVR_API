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
    public static class PasswordResetFunctions
    {
        [FunctionName("CreatePasswordReset")]
        public static async Task<IActionResult> CreatePasswordReset(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "passwordReset")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function(CreatePasswordReset) processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var passwordReset = JsonConvert.DeserializeObject<PasswordReset>(requestBody);

            try
            {
                var passwordResetRepo = new PasswordResetRepository();
                var newId = passwordResetRepo.CreatePasswordReset(passwordReset);

                return new OkObjectResult($"PasswordReset created with id {newId}.");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("GetPasswordResetById")]
        public static async Task<IActionResult> GetPasswordResetById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "passwordReset/{PasswordResetId}")] HttpRequest req, ILogger log, Guid passwordResetId)
        {
            log.LogInformation("C# HTTP trigger function(GetPasswordResetById) processed a request.");

            try
            {
                var passwordResetRepo = new PasswordResetRepository();
                var passwordReset = passwordResetRepo.GetPasswordResetById(passwordResetId);

                return new OkObjectResult(passwordReset);
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("DeletePasswordReset")]
        public static async Task<IActionResult> DeletePasswordReset(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "passwordReset/{PasswordResetId}")] HttpRequest req, ILogger log, Guid passwordResetId)
        {
            log.LogInformation("C# HTTP trigger function(DeletePasswordReset) processed a request.");

            try
            {
                var passwordResetRepo = new PasswordResetRepository();
                passwordResetRepo.DeletePasswordReset(passwordResetId);

                return new OkObjectResult($"Deleted {passwordResetId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        [FunctionName("UsePasswordReset")]
        public static async Task<IActionResult> UsePasswordReset(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "passwordReset/{PasswordResetId}")] HttpRequest req, ILogger log, Guid passwordResetId)
        {
            log.LogInformation("C# HTTP trigger function(UsePasswordReset) processed a request.");

            try
            {
                var passwordResetRepo = new PasswordResetRepository();
                passwordResetRepo.UsePasswordReset(passwordResetId);

                // now we have to actually add a row to organisation membership

                return new OkObjectResult($"Used Organisation Invite: {passwordResetId}");
            }
            catch (Exception exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
