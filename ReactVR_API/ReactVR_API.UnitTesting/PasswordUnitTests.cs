using ReactVR_API.Core.Security.Login;
using System;
using Xunit;

namespace ReactVR_API.UnitTesting
{
    public class PasswordUnitTesting
    {
        [Fact]
        public void Untampered_hash_matches_the_text()
        {
            // Arrange  
            var password = "thisisapassword";
            var salt = PasswordManager.GenerateSalt();
            var hash = PasswordManager.HashPassword(password, salt);

            // Act  
            var match = PasswordManager.ValidatePassword(password, salt, hash);

            // Assert  
            Assert.True(match);
        }

        [Fact]
        public void Tampered_hash_does_not_matche_the_text()
        {
            // Arrange  
            var password = "thisisapassword";
            var salt = PasswordManager.GenerateSalt();
            var hash = "tamperedhash";

            // Act  
            var match = PasswordManager.ValidatePassword(password, salt, hash);

            // Assert  
            Assert.False(match);
        }

        [Fact]
        public void Hash_of_two_different_messages_dont_match()
        {
            // Arrange  
            var password = "thisisapassword";
            var differentPassword = "thisisadifferentpassword";
            var salt = PasswordManager.GenerateSalt();

            // Act  
            var hash1 = PasswordManager.HashPassword(password, salt);
            var hash2 = PasswordManager.HashPassword(differentPassword, salt);

            // Assert  
            Assert.True(hash1 != hash2);
        }


        [Fact]
        public void Different_salts_generated()
        {        
            // Arrange  
            // Act
            var salt1 = PasswordManager.GenerateSalt();
            var salt2 = PasswordManager.GenerateSalt();

            // Assert  
            Assert.True(salt1 != salt2);
        }

        [Fact]
        public void Identical_passwords_have_different_hash()
        {             
            // Arrange  
            var password = "identicalpassword";
            var identicalPassword = "identicalpassword";

            //Act 
            var salt = PasswordManager.GenerateSalt();
            var hash1 = PasswordManager.HashPassword(password, salt);

            salt = PasswordManager.GenerateSalt();
            var hash2 = PasswordManager.HashPassword(identicalPassword, salt);

            // Assert  
            Assert.True(hash1 != hash2);
        }

    }
}
