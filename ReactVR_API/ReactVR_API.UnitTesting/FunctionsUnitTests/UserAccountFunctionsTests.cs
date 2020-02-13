using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ReactVR_API.Core.Functions;
using ReactVR_API.UnitTesting.HelperClasses;
using ReactVR_UnitTesting.FunctionsUnitTests.Boilerplate;
using ReactVR_UnitTesting.HelperClasses;
using Xunit;

namespace ReactVR_UnitTesting.FunctionsUnitTests
{
    public class UserAccountFunctionsTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        // valid jwt for my email address = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjI4ZDFjNGFkLTFmOTQtNDRiYS04NGYyLTE1NzFmM2EwMGE1ZCIsImV4cCI6MTU4Mzk3MDEwMCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.92TsVkYfv9YcMDFscN37zgFitDb7-VCWw2U0IdhnoCY"
        // jwt for "Invitee Guy" = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ2MTQ2ZWNmLWIzYTQtNDYwOC1hZDkwLTg2YjFjZGZjZjg4MCIsImV4cCI6MTU4NDEzNzc1MSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.9MxouRPj0AxhMq-z60VzZ9LT2vKZMOy9VLuvleeDsLg"
        [Fact]
        public async void CreateUserAccountTest()
        {
            var createModel = new
            {
                Name = "Invitee Guy",
                EmailAddress = DataMocking.RandomString(10) + "@reactvr.com",
                Password = "password",
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(createModel);

            var userAccountFunctions = new UserAccountFunctions();
            ObjectResult response = (ObjectResult)await userAccountFunctions.CreateUserAccount(mockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async void ChangeEmailAddressTest()
        {
            var randomEmailAddress = DataMocking.RandomString(10) + "@reactvr.com";
            var newRandomEmailAddress = DataMocking.RandomString(10) + "@reactvr.com";

            var updateModel = new
            {
                Name = "Jane Doe",
                EmailAddress = randomEmailAddress,
                Password = "password",
                NewEmailAddress = newRandomEmailAddress
            };

            Mock<HttpRequest> createMockRequest = MoqHelper.CreateMockRequest(updateModel);
            Mock<HttpRequest> changeMockRequest = MoqHelper.CreateMockRequest(updateModel);

            var userAccountFunctions = new UserAccountFunctions();
            ObjectResult createResponse = (ObjectResult)await userAccountFunctions.CreateUserAccount(createMockRequest.Object, logger);
            ObjectResult response = (ObjectResult)await userAccountFunctions.ChangeEmailAddress(changeMockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async void ChangePassword()
        {
            var updateModel = new
            {
                Name = "Jack",
                EmailAddress = "jackelliottorr@gmail.com",
                Password = "password",
                NewPassword = "password1",
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(updateModel);

            var userAccountFunctions = new UserAccountFunctions();
            ObjectResult response = (ObjectResult)await userAccountFunctions.ChangePassword(mockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }
    } 
}

