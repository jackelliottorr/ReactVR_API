using Dapper;
using ReactVR_API.HelperClasses;
using ReactVR_CORE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Repositories
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
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, organisation.GetType().Name, "OrganisationId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public Organisation GetOrganisationByOrganisationId(Guid organisationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { organisationId };
                var sql = "select * from organisation where organisationid = @organisationId";

                var organisation = db.QuerySingle<Organisation>(sql, parameters);

                return organisation;
            }
        }

        public void UpdateOrganisation(Organisation organisation)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    organisation.OrganisationName
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, organisation.GetType().Name);
                sql += " WHERE OrganisationId = @OrganisationId";

                db.Execute(sql, organisation);
            }
        }

        public void DeleteOrganisation(Guid organisationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    OrganisationId = organisationId
                };
                var sql = "update organisation set [IsDeleted] = 1 WHERE [OrganisationId] = @OrganisationId";

                db.Execute(sql, parameters);
            }
        }
    }
}
