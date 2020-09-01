using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReactVR_UnitTesting.FunctionsUnitTests.Boilerplate;
using ReactVR_UnitTesting.HelperClasses;
using Xunit;
using ReactVR_API.Core.Functions;

namespace ReactVR_API.UnitTesting.FunctionsUnitTests
{
    public class ScoreboardUnitTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreatScoreboardEntry()
        {
            // After accepting invite
            string inviteAcceptedJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6ImNhNDRkNzhkLWZlZTktNDMwNS1hNWFkLWU4ZTc3ODZkOTA5ZSIsIk9yZ2FuaXNhdGlvbiI6ImY2MjU5ZGE4LWZkZmQtNDA1Yy1iM2RjLTZkMDVlNjA0NTAzZiIsImV4cCI6MTU4NDE5Mzg4MywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.V6PWzQY0I7WoDm97ULxlDExa70tdyVaYR87S7vS82X8";

            var createModel = new
            {
                LevelConfigurationId = "08D53ECB-C1F1-4B55-A6F9-A1456A37289F",
                Score = 1
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(createModel, inviteAcceptedJWT);

            var organisationInviteFunctions = new ScoreboardFunctions();
            ObjectResult response = (ObjectResult)await organisationInviteFunctions.CreateScoreboardEntry(mockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async void GetScoreboard()
        {
            // After accepting invite
            //string inviteAcceptedJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6ImNhNDRkNzhkLWZlZTktNDMwNS1hNWFkLWU4ZTc3ODZkOTA5ZSIsIk9yZ2FuaXNhdGlvbiI6ImY2MjU5ZGE4LWZkZmQtNDA1Yy1iM2RjLTZkMDVlNjA0NTAzZiIsImV4cCI6MTU4NDE5Mzg4MywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.V6PWzQY0I7WoDm97ULxlDExa70tdyVaYR87S7vS82X8";

            //var model = new
            //{
            //    LevelConfigurationId = "08D53ECB-C1F1-4B55-A6F9-A1456A37289F"
            //};

            //Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(model, inviteAcceptedJWT);

            //var organisationInviteFunctions = new ScoreboardFunctions();
            //ObjectResult response = (ObjectResult)await organisationInviteFunctions.GetScoreboardForLevelConfiguration(mockRequest.Object, logger, model.LevelConfigurationId);

            //Assert.True(response.StatusCode == 200);
        }
    }
}
