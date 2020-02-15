using Dapper;
using ReactVR_API.Core.HelperClasses;
using ReactVR_API.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Core.Repositories
{
    class ScoreboardRepository : BaseRepository
    {
        public Guid CreateScoreboard(Scoreboard scoreboard)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    scoreboard.UserAccountId,
                    scoreboard.LevelConfigurationId,
                    scoreboard.Score
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, scoreboard.GetType().Name, "ScoreboardId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public Scoreboard GetScoreboardById(Guid scoreboardId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { scoreboardId };
                var sql = "select * from scoreboard where scoreboardid = @scoreboardId";

                var scoreboard = db.QuerySingle<Scoreboard>(sql, parameters);

                return scoreboard;
            }
        }

        //public void UpdateScoreboard(Scoreboard scoreboard)
        //{
        //    using (var db = new SqlConnection(_connectionString))
        //    {
        //        var parameters = new
        //        {
        //            scoreboard.Name,
        //            scoreboard.Description
        //        };

        //        var sql = SqlCrudHelper.GetUpdateStatement(parameters, scoreboard.GetType().Name);
        //        sql += " WHERE ScoreboardId = @ScoreboardId";

        //        db.Execute(sql, scoreboard);
        //    }
        //}

        public void DeleteScoreboard(Guid scoreboardId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    ScoreboardId = scoreboardId
                };
                var sql = "update scoreboard set [IsDeleted] = 1 WHERE [ScoreboardId] = @ScoreboardId";

                db.Execute(sql, parameters);
            }
        }

        public List<ScoreboardViewModel> GetScoreboardForLevelConfiguration(Guid levelConfigurationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    LevelConfigurationId = levelConfigurationId
                };
                var sql = "select top (100) u.Name, s.Score, s.CreatedDate " +
                          "from scoreboard s " +
                          "inner join useraccount u " +
                          "on s.UserAccountId = u.UserAccountId " +
                          "where s.LevelConfigurationId = @LevelConfigurationId";

                var scores = (List<ScoreboardViewModel>)db.Query<ScoreboardViewModel>(sql, parameters);

                return scores;
            }
        }
    }
}
