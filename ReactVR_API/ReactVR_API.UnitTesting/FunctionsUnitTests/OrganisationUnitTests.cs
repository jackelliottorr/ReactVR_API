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
    public class OrganisationUnitTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        private string _name = "Jack";
        private string _emailAddress = "jackelliottorr@gmail.com";
        private string _password = "password1";
        private string _jwt = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjI4ZDFjNGFkLTFmOTQtNDRiYS04NGYyLTE1NzFmM2EwMGE1ZCIsImV4cCI6MTU4Mzk3MDEwMCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.92TsVkYfv9YcMDFscN37zgFitDb7-VCWw2U0IdhnoCY";

        private string _organisationJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6IjI4ZDFjNGFkLTFmOTQtNDRiYS04NGYyLTE1NzFmM2EwMGE1ZCIsIk9yZ2FuaXNhdGlvbiI6ImY2MjU5ZGE4LWZkZmQtNDA1Yy1iM2RjLTZkMDVlNjA0NTAzZiIsImV4cCI6MTU4NDAzMTQ1NCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.zhJ0gDVYDG3toS3yGY-LiM-0v30vZPHuLSjL71gDmPs";
        private string _organisation2JWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6IjI4ZDFjNGFkLTFmOTQtNDRiYS04NGYyLTE1NzFmM2EwMGE1ZCIsIk9yZ2FuaXNhdGlvbiI6Ijk4OWM2MWY1LWRlNjctNDVmMy1hZDBlLTVmMmRiNzI1ZTg5YSIsImV4cCI6MTU4NDAzMTUxMiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.u8BGJGYl4XU9g9FF9JshEvv37ykPvyBFB755M_c7iZo";

        [Fact]
        public async void CreateOrganisation()
        {
            var createModel = new
            {
                OrganisationName = "My Second Organisation"
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(createModel, _jwt);
            
            var organisationFunctions = new OrganisationFunctions();
            ObjectResult response = (ObjectResult)await organisationFunctions.CreateOrganisation(mockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async void GetOrganisationsForUser()
        {
            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(_organisationJWT);

            var organisationFunctions = new OrganisationFunctions();
            ObjectResult response = (ObjectResult)await organisationFunctions.GetOrganisationsForUser(mockRequest.Object, logger);
            
            Assert.True(response.StatusCode == 200);
        }
    }
}
