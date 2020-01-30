using ReactVR_API.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.HelperClasses
{
    class LoginManager
    {
        public LoginManager()
        {
        }

        public Boolean AccountExists(string emailAddress)
        {
            var userAccountRepo = new UserAccountRepository();

            bool accountExists = userAccountRepo.AccountExists(emailAddress);

            return accountExists;
        }

        public Boolean CheckPassword(string emailAddress, string password)
        {

            return true;
        }
    }
}
