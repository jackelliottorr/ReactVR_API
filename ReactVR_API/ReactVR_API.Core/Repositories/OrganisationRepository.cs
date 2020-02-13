using Dapper;
using ReactVR_API.Core.HelperClasses;
using ReactVR_API.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Core.Repositories
{
    class OrganisationRepository : BaseRepository
    {
        public Guid CreateOrganisation(Organisation organisation)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    organisation.OrganisationName,
                    organisation.CreatedById
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, organisation.GetType().Name, "OrganisationId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public Organisation GetOrganisationById(Guid organisationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { organisationId };
                var sql = "select * from organisation where organisationid = @organisationId";

                var organisation = db.QuerySingle<Organisation>(sql, parameters);

                return organisation;
            }
        }

        public List<Organisation> GetOrganisationsForUser(Guid userAccountId)
        {
            var organisations = new List<Organisation>();

            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { userAccountId };
                var sql = "select * from Organisation o " +
                            "join OrganisationMembership om " +
                            "on o.OrganisationId = om.OrganisationId " +
                            "where om.UserAccountId = @UserAccountId";

                organisations = (List<Organisation>)db.Query<Organisation>(sql, parameters);
            }

            return organisations;
        }

        public bool UpdateOrganisation(Organisation organisation)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    organisation.OrganisationName
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, organisation.GetType().Name);
                sql += " WHERE OrganisationId = @OrganisationId";

                var result = db.Execute(sql, parameters);
                var boolResult = result == 1 ? true : false;
                return boolResult;
            }
        }

        public bool DeleteOrganisation(Guid organisationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    OrganisationId = organisationId
                };
                var sql = "update organisation set [IsDeleted] = 1 WHERE [OrganisationId] = @OrganisationId";

                var result = db.Execute(sql, parameters);
                var boolResult = result == 1 ? true : false;
                return boolResult;
            }
        }
    }
}
