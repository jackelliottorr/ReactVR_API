using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReactVR_API.Functions;
using ReactVR_UnitTesting.FunctionsUnitTests.Boilerplate;
using ReactVR_UnitTesting.HelperClasses;
using Xunit;

namespace ReactVR_UnitTesting.FunctionsUnitTests
{
    public class UserAccountFunctionsTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void ChangeEmailAddressTest()
        {
            var createModel = new
            {
                Name = "Jack",
                EmailAddress = "",
                NewEmailAddress = "newEmailAddress@test.com",
                Password = "password",
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(createModel);
           
            var userAccountFunctions = new UserAccountFunctions();
            var response = (OkObjectResult)await userAccountFunctions.ChangeEmailAddress(mockRequest.Object, logger);

            Assert.Contains("Email Address updated to", response.Value.ToString());
        }

        [Fact]
        public async void ChangePassword()
        {
            var createModel = new
            {
                Name = "Jack",
                EmailAddress = "newEmailAddress@test.com",
                Password = "password",
                NewPassword = "password1",
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(createModel);

            var userAccountFunctions = new UserAccountFunctions();
            var response = (OkObjectResult)await userAccountFunctions.ChangePassword(mockRequest.Object, logger);

            Assert.Contains("Password updated.", response.Value.ToString());
        }
    } 
}

