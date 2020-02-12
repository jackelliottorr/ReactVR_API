using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ReactVR_API.Core.Security.Login
{
    public class PasswordManager
    {
        /// <summary>
        /// Hashs the password with our generated salt and returns the hash
        /// These can both then be saved in the database
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>Hashed password</returns>
        public static string HashPassword(string password, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(salt),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        public static bool ValidatePassword(string value, string salt, string hash)
            => HashPassword(value, salt) == hash;

        /// <summary>
        /// Generate a unique, cryptographically-strong salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string GenerateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }
    }
}

