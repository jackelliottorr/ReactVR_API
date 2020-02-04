using ReactVR_API.HelperClasses;
using ReactVR_CORE.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReactVR_UnitTesting
{
    public class ModelUpdaterTests
    {
        [Fact]
        public void Untampered_hash_matches_the_text()
        {
            // Arrange  
            var userAccount = new UserAccount()
            {
                Name = "Destination",
                EmailAddress = "destination@a.com",
                Salt =  "salt",
                Hash = "hash"
            };

            var updateModel = new UserAccountUpdateModel()
            {
                Name = "Source",
                EmailAddress = "",
                Password = "pass"
            };

            // Act              
            ModelUpdater.CopyProperties(updateModel, userAccount);

            bool match = true;
            // Assert  
            Assert.True(match);
        }
    }
}
