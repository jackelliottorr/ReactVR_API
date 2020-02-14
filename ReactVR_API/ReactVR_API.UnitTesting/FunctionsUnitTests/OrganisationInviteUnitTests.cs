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
                InviteeEmailAddress = "TJ9ZX3C78V@reactvr.com",
                InviteUserType = 4
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
            string inviteeJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6ImNhNDRkNzhkLWZlZTktNDMwNS1hNWFkLWU4ZTc3ODZkOTA5ZSIsImV4cCI6MTU4NDE5MzA5OCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.sU3R5zFbXB9Mala9z5Kio4sFaXYLTS8ohdOn_b0KMQM";

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(inviteeJWT);

            var organisationInviteFunctions = new OrganisationInviteFunctions();
            ObjectResult response = (ObjectResult)await organisationInviteFunctions.GetInvitesForUser(mockRequest.Object, logger);

            Assert.True(response.StatusCode == 200);
        }

        [Fact]
        public async void AcceptOrganisationInvite()
        {            
            // Before accepting invite
            string inviteeJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6ImNhNDRkNzhkLWZlZTktNDMwNS1hNWFkLWU4ZTc3ODZkOTA5ZSIsImV4cCI6MTU4NDE5MzA5OCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.sU3R5zFbXB9Mala9z5Kio4sFaXYLTS8ohdOn_b0KMQM";

            var model = new
            {
                OrganisationInviteId = "e06d2bb2-f6da-43c7-b4d1-147b2a1c0620"
            };

            Mock<HttpRequest> mockRequest = MoqHelper.CreateMockRequest(model, inviteeJWT);

            var organisationInviteFunctions = new OrganisationInviteFunctions();
            ObjectResult response = (ObjectResult)await organisationInviteFunctions.AcceptOrganisationInvite(mockRequest.Object, logger);

            // After accepting invite
            string _inviteAcceptedJWT = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyQWNjb3VudCI6ImNhNDRkNzhkLWZlZTktNDMwNS1hNWFkLWU4ZTc3ODZkOTA5ZSIsIk9yZ2FuaXNhdGlvbiI6ImY2MjU5ZGE4LWZkZmQtNDA1Yy1iM2RjLTZkMDVlNjA0NTAzZiIsImV4cCI6MTU4NDE5Mzg4MywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo3MDcxIiwiYXVkIjoiTXlBdWRpZW5jZSJ9.V6PWzQY0I7WoDm97ULxlDExa70tdyVaYR87S7vS82X8";

            Assert.True(response.StatusCode == 200);
        }
    }
}
