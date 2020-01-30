using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactVR_API.HelperClasses;
using Xunit.Sdk;

namespace UnitTesting
{
    [TestClass]
    public class PasswordUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        //[Fact(DisplayName = "Untampered_hash_matches_the_text")]
        [TestMethod]
        public void Untampered_hash_matches_the_text()
        {
            // Arrange  
            var message = "passw0rd";
            var salt = SaltManager.Create();
            var hash = HashManager.Create(message, salt);

            // Act  
            var match = HashManager.Validate(message, salt, hash);

            // Assert  
            Assert.True(match);
        }
    }
}
