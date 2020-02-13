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
    public class OrganisationInviteUnitTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        private string _name = "Jack";
        private string _emailAddress = "jackelliottorr@gmail.com";
        private string _password = "password1";
        private string _jwt = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjI4ZDFjNGFkLTFmOTQtNDRiYS04NGYyLTE1NzFmM2EwMGE1ZCIsImV4cCI6MTU4Mzk3MDEwMCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.92TsVkYfv9YcMDFscN37zgFitDb7-VCWw2U0IdhnoCY";

        private string _renamedOrganisationJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6IjI4ZDFjNGFkLTFmOTQtNDRiYS04NGYyLTE1NzFmM2EwMGE1ZCIsIk9yZ2FuaXNhdGlvbiI6ImY2MjU5ZGE4LWZkZmQtNDA1Yy1iM2RjLTZkMDVlNjA0NTAzZiIsImV4cCI6MTU4NDAzMTQ1NCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.zhJ0gDVYDG3toS3yGY-LiM-0v30vZPHuLSjL71gDmPs";
        private string _mySecondOrganisationJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6IjI4ZDFjNGFkLTFmOTQtNDRiYS04NGYyLTE1NzFmM2EwMGE1ZCIsIk9yZ2FuaXNhdGlvbiI6Ijk4OWM2MWY1LWRlNjctNDVmMy1hZDBlLTVmMmRiNzI1ZTg5YSIsImV4cCI6MTU4NDAzMTUxMiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.u8BGJGYl4XU9g9FF9JshEvv37ykPvyBFB755M_c7iZo";

        [Fact]
        public async void CreateOrganisationInvite()
        {
            var createModel = new
            {
                InviteeEmailAddress = "UGO4F6NHQA@reactvr.com"
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(createModel, _renamedOrganisationJWT);

            var organisationInviteFunctions = new OrganisationInviteFunctions();
            ObjectResult response = (ObjectResult)await organisationInviteFunctions.CreateOrganisationInvite(mockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async void GetOrganisationInvites()
        {
            // Before accepting invite
            string _inviteeJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ2MTQ2ZWNmLWIzYTQtNDYwOC1hZDkwLTg2YjFjZGZjZjg4MCIsImV4cCI6MTU4NDEzNzc1MSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.9MxouRPj0AxhMq-z60VzZ9LT2vKZMOy9VLuvleeDsLg";

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(_inviteeJWT);

            var organisationInviteFunctions = new OrganisationInviteFunctions();
            ObjectResult response = (ObjectResult)await organisationInviteFunctions.CreateOrganisationInvite(mockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async void AcceptOrganisationInvite()
        {
            var model = new
            {
                OrganisationInviteId = ""
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(model, _renamedOrganisationJWT);

            var organisationInviteFunctions = new OrganisationInviteFunctions();
            ObjectResult response = (ObjectResult)await organisationInviteFunctions.CreateOrganisationInvite(mockRequest.Object, logger);

            // After accepting invite
            string _inviteAcceptedJWT = "Bearer ";

            Assert.True(response.StatusCode == 200);
        }
    }
}
