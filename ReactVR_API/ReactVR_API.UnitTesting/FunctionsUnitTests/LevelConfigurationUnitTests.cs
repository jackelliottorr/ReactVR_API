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
    public class LevelConfigurationUnitTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void GetLevelConfigurations()
        {
            // After accepting invite
            string inviteAcceptedJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6ImNhNDRkNzhkLWZlZTktNDMwNS1hNWFkLWU4ZTc3ODZkOTA5ZSIsIk9yZ2FuaXNhdGlvbiI6ImY2MjU5ZGE4LWZkZmQtNDA1Yy1iM2RjLTZkMDVlNjA0NTAzZiIsImV4cCI6MTU4NDE5Mzg4MywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.V6PWzQY0I7WoDm97ULxlDExa70tdyVaYR87S7vS82X8";
            Guid levelId = new Guid("A5460F25-BB2C-4E37-A0B8-6329CAE85D96");

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(inviteAcceptedJWT);

            var levelConfigurationFunctions = new LevelConfigurationFunctions();
            ObjectResult response = (ObjectResult)await levelConfigurationFunctions.GetLevelConfigurationsByLevelId(mockRequest.Object, logger, levelId);

            Assert.True(response.StatusCode == 200);
        }
    }
}
