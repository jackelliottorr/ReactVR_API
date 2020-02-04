using ReactVR_CORE.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Security.Login
{
    /// <summary>
    /// Contains the result of a Login attempt.
    /// </summary>
    public sealed class LoginResult
    {
        private LoginResult() { }

        /// <summary>
        /// Return UserAccount after successful login
        /// </summary>
        public UserAccount UserAccount
        { get; private set; }

        /// <summary>
        /// Gets the status of the login, i.e. whether it is valid.
        /// </summary>
        public LoginStatus Status
        { get; private set; }

        public string FailureReason
        {
            get; private set;
        }

        /// <summary>
        /// Gets any exception encountered when validating a token.
        /// </summary>
        public Exception Exception
        { get; private set; }

        public static LoginResult Success(UserAccount userAccount)
        {
            return new LoginResult
            {
                UserAccount = userAccount,
                Status = LoginStatus.Success
            };
        }

        /// <summary>
        /// Returns a result that indicates the login failed.
        /// </summary>
        public static LoginResult Failure(string failureReason)
        {
            return new LoginResult
            {
                FailureReason = failureReason,
                Status = LoginStatus.Failure
            };
        }

        /// <summary>
        /// Returns a result to indicate that there was an error when logging in.
        /// </summary>
        public static LoginResult Error(Exception ex)
        {
            return new LoginResult
            {
                Status = LoginStatus.Error,
                FailureReason = ex.Message,
                Exception = ex
            };
        }
    }
}
