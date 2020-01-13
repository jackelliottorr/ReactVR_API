using Dapper;
using ReactVR_API.HelperClasses;
using ReactVR_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReactVR_API.Repositories
{
    class LevelConfigurationRepository : BaseRepository
    {
        public Guid CreateLevelConfiguration(LevelConfiguration levelConfiguration)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    levelConfiguration.LevelId,
                    levelConfiguration.OrganisationId,
                    levelConfiguration.CreatedById,
                    levelConfiguration.TargetZoneId,
                    levelConfiguration.Name,
                    levelConfiguration.Description,
                    levelConfiguration.TargetSpawnDelay,
                    levelConfiguration.TargetLifespan,
                    levelConfiguration.IsPublic
                };

                var sql = SqlCrudHelper.GetInsertStatement(parameters, levelConfiguration.GetType().Name, "LevelConfigurationId");

                Guid newId = db.ExecuteScalar<Guid>(sql, parameters);

                return newId;
            }
        }

        /// <summary>
        /// Include other ways to get Level Configurations
        /// For example, by organisation, by public, by level, by name??
        /// </summary>
        /// <param name="levelConfigurationId"></param>
        /// <returns></returns>
        public LevelConfiguration GetLevelConfigurationById(Guid levelConfigurationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { levelConfigurationId };
                var sql = "select * from levelConfiguration where levelConfigurationid = @levelConfigurationId";

                var levelConfiguration = db.QuerySingle<LevelConfiguration>(sql, parameters);

                return levelConfiguration;
            }
        }

        public void UpdateLevelConfiguration(LevelConfiguration levelConfiguration)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    levelConfiguration.Name,
                    levelConfiguration.Description,
                    levelConfiguration.TargetSpawnDelay,
                    levelConfiguration.TargetLifespan,
                    levelConfiguration.IsPublic
                };

                var sql = SqlCrudHelper.GetUpdateStatement(parameters, levelConfiguration.GetType().Name);
                sql += " WHERE LevelConfigurationId = @LevelConfigurationId";

                db.Execute(sql, levelConfiguration);
            }
        }

        public void DeleteLevelConfiguration(Guid levelConfigurationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    LevelConfigurationId = levelConfigurationId
                };
                var sql = "update levelConfiguration set [IsDeleted] = 1 WHERE [LevelConfigurationId] = @LevelConfigurationId";

                db.Execute(sql, parameters);
            }
        }
    }
}
