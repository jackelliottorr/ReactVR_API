using Dapper;
using ReactVR_API.HelperClasses;
using ReactVR_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Repositories
{
    class PasswordResetRepository : BaseRepository
    {
        public Guid CreatePasswordReset(PasswordReset passwordReset)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    passwordReset.UserAccountId
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, passwordReset.GetType().Name, "PasswordResetId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public PasswordReset GetPasswordResetById(Guid passwordResetId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { passwordResetId };
                var sql = "select * from passwordReset where passwordResetid = @passwordResetId";

                var passwordReset = db.QuerySingle<PasswordReset>(sql, parameters);

                return passwordReset;
            }
        }

        public void DeletePasswordReset(Guid passwordResetId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    PasswordResetId = passwordResetId
                };
                var sql = "update PasswordReset set [IsDeleted] = 1 WHERE [PasswordResetId] = @PasswordResetId";

                db.Execute(sql, parameters);
            }
        }

        public void UsePasswordReset(Guid passwordResetId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    PasswordResetId = passwordResetId
                };
                var sql = "update passwordReset set [IsUsed] = 1 WHERE [PasswordResetId] = @PasswordResetId";

                db.Execute(sql, parameters);
            }
        }
    }
}
