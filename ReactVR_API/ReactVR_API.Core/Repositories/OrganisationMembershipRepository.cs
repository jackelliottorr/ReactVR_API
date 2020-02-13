using Dapper;
using ReactVR_API.Core.HelperClasses;
using ReactVR_API.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Core.Repositories
{
    class OrganisationMembershipRepository : BaseRepository
    {
        public Guid CreateOrganisationMembership(OrganisationMembership organisationMembership)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    organisationMembership.OrganisationId,
                    organisationMembership.UserAccountId,
                    organisationMembership.UserType,
                    organisationMembership.OrganisationInviteId
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, organisationMembership.GetType().Name, "OrganisationMembershipId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public OrganisationMembership GetOrganisationMembershipById(Guid organisationMembershipId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { organisationMembershipId };
                var sql = "select * from organisationMembership where organisationMembershipid = @organisationMembershipId";

                var organisationMembership = db.QuerySingle<OrganisationMembership>(sql, parameters);

                return organisationMembership;
            }
        }

        public OrganisationMembership GetOrganisationMembership(Guid userAccountId, Guid organisationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { userAccountId, organisationId };
                var sql = "select * from organisationmembership where useraccountid = @useraccountid and organisationid = @organisationid";

                var organisationMembership = db.QuerySingleOrDefault<OrganisationMembership>(sql, parameters);

                return organisationMembership;
            }
        }

        /// <summary>
        /// Only for changing a user's role in the organisation
        /// </summary>
        /// <param name="organisationMembership"></param>
        public void UpdateOrganisationMembership(OrganisationMembership organisationMembership)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    organisationMembership.UserType
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, organisationMembership.GetType().Name);
                sql += " WHERE OrganisationMembershipId = @OrganisationMembershipId";

                db.Execute(sql, organisationMembership);
            }
        }
        
        /// <summary>
        /// Remove a user from an organisation
        /// </summary>
        /// <param name="organisationMembershipId"></param>
        public void DeleteOrganisationMembership(Guid organisationMembershipId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    OrganisationMembershipId = organisationMembershipId
                };
                var sql = "update organisationMembership set [IsDeleted] = 1 WHERE [OrganisationMembershipId] = @OrganisationMembershipId";

                db.Execute(sql, parameters);
            }
        }

        public bool AlreadyHasAMembershipInOrganisation(Guid userAccountId, Guid organisationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    UserAccountId = userAccountId,
                    OrganisationId = organisationId
                };

                var sql = "select case when exists " +
                "(select * from organisationmembership where useraccountid = @UserAccountId and organisationid = @OrganisationId) " +
                "THEN CAST(1 AS BIT)" +
                "ELSE CAST(0 AS BIT) END";

                int result = db.QuerySingle<int>(sql, parameters);

                bool accountExists = result == 1 ? true : false;
                return accountExists;
            }
        }
    }
}
