using ReactVR_API.HelperClasses;
using ReactVR_API.Repositories;
using ReactVR_API.Security.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Security.Login
{
    class LoginManager
    {
        public LoginManager()
        {
        }

        public LoginResult AttemptLogin(string emailAddress, string password)
        {
            try
            {
                if (!AccountExists(emailAddress))
                {
                    return LoginResult.Failure("Email Address not found");
                }

                // get UserAccount from database using the EmailAdress (to get salt & hash)
                var userAccountRepo = new UserAccountRepository();
                var userAccount = userAccountRepo.GetUserAccountByEmailAddress(emailAddress);

                // validate password against salt & hash
                if (!PasswordManager.ValidatePassword(password, userAccount.Salt, userAccount.Hash))
                {
                    return LoginResult.Failure("Wrong password");
                }

                return LoginResult.Success(userAccount);
            }
            catch (Exception exception)
            {
                return LoginResult.Error(exception);
            }
        }

        public Boolean AccountExists(string emailAddress)
        {
            var userAccountRepo = new UserAccountRepository();

            bool accountExists = userAccountRepo.AccountExists(emailAddress);

            return accountExists;
        }
    }
}
