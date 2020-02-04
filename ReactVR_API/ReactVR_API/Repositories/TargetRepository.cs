using Dapper;
using ReactVR_API.HelperClasses;
using ReactVR_CORE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Repositories
{
    class TargetRepository : BaseRepository
    {
        public Guid CreateTarget(Target target)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    target.LevelConfigurationId,
                    target.TargetShape,
                    target.TargetX,
                    target.TargetY,
                    target.TargetZ,
                    target.OffsetX,
                    target.OffsetY,
                    target.OffsetZ
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, target.GetType().Name, "TargetId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public Target GetTargetById(Guid targetId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { targetId };
                var sql = "select * from target where targetid = @targetId";

                var target = db.QuerySingle<Target>(sql, parameters);

                return target;
            }
        }

        public void UpdateTarget(Target target)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    target.TargetShape,
                    target.TargetX,
                    target.TargetY,
                    target.TargetZ,
                    target.OffsetX,
                    target.OffsetY,
                    target.OffsetZ
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, target.GetType().Name);
                sql += " WHERE TargetId = @TargetId";

                db.Execute(sql, target);
            }
        }

        public void DeleteTarget(Guid targetId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    TargetId = targetId
                };
                var sql = "update target set [IsDeleted] = 1 WHERE [TargetId] = @TargetId";

                db.Execute(sql, parameters);
            }
        }
    }
}
