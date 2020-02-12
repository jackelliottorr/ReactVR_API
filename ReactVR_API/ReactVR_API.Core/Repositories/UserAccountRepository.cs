using Dapper;
using ReactVR_API.Core.HelperClasses;
using ReactVR_API.Common.Models;
using System;
using System.Data.SqlClient;

namespace ReactVR_API.Core.Repositories
{
    class UserAccountRepository : BaseRepository
    {
        public Guid CreateUserAccount(UserAccount userAccount)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    userAccount.UserAccountId,
                    userAccount.Name,
                    userAccount.EmailAddress,
                    userAccount.Salt,
                    userAccount.Hash,
                    userAccount.CreatedDate,
                    userAccount.IsDeleted
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, userAccount.GetType().Name, "UserAccountId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public UserAccount GetUserAccountById(Guid userAccountId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserAccountId = userAccountId };
                var sql = "select * from useraccount where useraccountid = @userAccountId";

                var userAccount = db.QuerySingle<UserAccount>(sql, parameters);
                return userAccount;
            }
        }

        internal UserAccount GetUserAccountByEmailAddress(string emailAddress)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { EmailAddress = emailAddress };
                var sql = "select * from useraccount where emailaddress = @EmailAddress";

                var userAccount = db.QuerySingle<UserAccount>(sql, parameters);
                return userAccount;
            }
        }

        public bool UpdateUserAccount(UserAccount userAccount)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    userAccount.Name,
                    userAccount.EmailAddress,
                    userAccount.Salt,
                    userAccount.Hash
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, userAccount.GetType().Name);
                sql += " WHERE UserAccountId = @UserAccountId";

                var result = db.Execute(sql, userAccount);

                var boolResult = result == 1 ? true : false;
                return boolResult;
            }
        }

        public bool DeleteUserAccount(Guid userAccountId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserAccountId = userAccountId };
                var sql = "delete from useraccount where [UserAccountId] = @UserAccountId";
                
                var result = db.Execute(sql, parameters);

                var boolResult = result == 1 ? true : false;
                return boolResult;
            }
        }

        public bool AccountExists(string emailAddress)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { EmailAddress = emailAddress };
                var sql = "select case when exists (";
                sql += " select * from [UserAccount] where [EmailAddress] = @EmailAddress)";     
                sql += " THEN CAST(1 AS BIT)";
                sql += " ELSE CAST(0 AS BIT) END";

                int result = db.QuerySingle<int>(sql, parameters);

                bool accountExists = result == 1 ? true : false;
                return accountExists;
            }
        }
    }
}
