using Dapper;
using ReactVR_API.HelperClasses;
using ReactVR_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Repositories
{
    class OrganisationInviteRepository : BaseRepository
    {
        public Guid CreateOrganisationInvite(OrganisationInvite organisationInvite)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    organisationInvite.OrganisationId,
                    organisationInvite.InvitedById,
                    organisationInvite.InviteUserType,
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, organisationInvite.GetType().Name, "OrganisationInviteId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public OrganisationInvite GetOrganisationInviteById(Guid organisationInviteId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { organisationInviteId };
                var sql = "select * from organisationInvite where organisationInviteid = @organisationInviteId";

                var organisationInvite = db.QuerySingle<OrganisationInvite>(sql, parameters);

                return organisationInvite;
            }
        }

        public void DeleteOrganisationInvite(Guid organisationInviteId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    OrganisationInviteId = organisationInviteId
                };
                var sql = "update organisationInvite set [IsDeleted] = 1 WHERE [OrganisationInviteId] = @OrganisationInviteId";

                db.Execute(sql, parameters);
            }
        }

        public void UseOrganisationInvite(Guid organisationInviteId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    OrganisationInviteId = organisationInviteId
                };
                var sql = "update organisationInvite set [IsUsed] = 1 WHERE [OrganisationInviteId] = @OrganisationInviteId";

                db.Execute(sql, parameters);
            }
        }
    }
}
