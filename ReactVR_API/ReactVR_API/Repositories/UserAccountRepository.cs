﻿using Dapper;
using ReactVR_API.HelperClasses;
using ReactVR_API.Models;
using System;
using System.Data.SqlClient;

namespace ReactVR_API.Repositories
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
                    userAccount.Password,
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

        public void UpdateUserAccount(UserAccount userAccount)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    userAccount.Name,
                    userAccount.EmailAddress,
                    userAccount.Password,
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, userAccount.GetType().Name);
                sql += " WHERE UserAccountId = @UserAccountId";

                db.Execute(sql, userAccount);

                //var result = db.Execute(sql, userAccount);
                //var boolResult = result == 1 ? true : false;
                //return boolResult;
            }
        }

        public bool DeleteUserAccount(Guid userAccountId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserAccountId = userAccountId };
                var sql = "update useraccount set [IsDeleted] = 1 where [UserAccountId] = @UserAccountId";
                
                var result = db.Execute(sql, parameters);

                var boolResult = result == 1 ? true : false;
                return boolResult;
            }
        }
    }
}