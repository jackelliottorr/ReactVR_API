using Dapper;
using ReactVR_API.HelperClasses;
using ReactVR_CORE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Repositories
{
    class TargetAppearanceRepository : BaseRepository
    {
        public Guid CreateTargetAppearance(TargetAppearance targetAppearance)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    targetAppearance.TargetId,
                    targetAppearance.ScoreboardId,
                    targetAppearance.TargetUptime,
                    targetAppearance.WasMissed
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, targetAppearance.GetType().Name, "TargetAppearanceId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public TargetAppearance GetTargetAppearanceById(Guid targetAppearanceId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { targetAppearanceId };
                var sql = "select * from targetAppearance where targetAppearanceid = @targetAppearanceId";

                var targetAppearance = db.QuerySingle<TargetAppearance>(sql, parameters);

                return targetAppearance;
            }
        }

        //public void UpdateTargetAppearance(TargetAppearance targetAppearance)
        //{
        //    using (var db = new SqlConnection(_connectionString))
        //    {
        //        var parameters = new
        //        {
        //            targetAppearance.TargetId,
        //            targetAppearance.ScoreboardId,
        //            targetAppearance.TargetUptime,
        //            targetAppearance.WasMissed
        //        };

        //        var sql = SqlCrudHelper.GetUpdateStatement(parameters, targetAppearance.GetType().Name);
        //        sql += " WHERE TargetAppearanceId = @TargetAppearanceId";

        //        db.Execute(sql, targetAppearance);
        //    }
        //}

        public void DeleteTargetAppearance(Guid targetAppearanceId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    TargetAppearanceId = targetAppearanceId
                };
                var sql = "update targetAppearance set [IsDeleted] = 1 WHERE [TargetAppearanceId] = @TargetAppearanceId";

                db.Execute(sql, parameters);
            }
        }
    }
}
