﻿using Dapper;
using ReactVR_API.Core.HelperClasses;
using ReactVR_API.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Core.Repositories
{
    class LevelRepository : BaseRepository
    {
        public Guid CreateLevel(Level level)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    level.Name,
                    level.Description
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, level.GetType().Name, "LevelId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        public Level GetLevelByLevelId(Guid levelId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { levelId };
                var sql = "select * from level where levelid = @levelId";

                var level = db.QuerySingle<Level>(sql, parameters);

                return level;
            }
        }

        public void UpdateLevel(Level level)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    level.Name,
                    level.Description
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, level.GetType().Name);
                sql += " WHERE LevelId = @LevelId";

                db.Execute(sql, level);
            }
        }

        public void DeleteLevel(Guid levelId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    LevelId = levelId
                };
                var sql = "update level set [IsDeleted] = 1 WHERE [LevelId] = @LevelId";

                db.Execute(sql, parameters);
            }
        }

        public List<Level> GetAllLevels()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = "select top (10) * from [Level]";
                var levels = (List<Level>)db.Query<Level>(sql);
                return levels;
            }
        }
    }
}
