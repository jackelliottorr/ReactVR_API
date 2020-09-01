using Dapper;
using ReactVR_API.Core.HelperClasses;
using ReactVR_API.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace ReactVR_API.Core.Repositories
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

        public List<LevelConfigurationViewModel> GetLevelConfigurationsByLevelId(Guid levelId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { LevelId = levelId };
                var sql = "select top (10) * from levelconfiguration where levelid = @LevelId";

                var levelConfigurations = (List<LevelConfigurationViewModel>)db.Query<LevelConfigurationViewModel>(sql, parameters);

                return levelConfigurations;
            }
        }

        public List<LevelConfigurationViewModel> GetLevelConfigurationsByCreatedById(Guid userAccountId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { UserAccountId = userAccountId };
                var sql = "select top (10) * from levelconfiguration where createdById = @UserAccountId";

                var levelConfigurations = (List<LevelConfigurationViewModel>)db.Query<LevelConfigurationViewModel>(sql, parameters);

                return levelConfigurations;
            }
        }

        public List<LevelConfiguration> TestDapper(Guid levelId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { LevelId = levelId };
                var sql = "select top (10) from levelconfiguration where levelid = @LevelId";

                var levelConfigurations = (List<LevelConfiguration>)db.Query<LevelConfiguration>(sql, parameters);

                return levelConfigurations;
            }
        }

        /// <summary>
        /// Include other ways to get Level Configurations
        /// For example, by organisation, by public, by level, by name??
        /// </summary>
        /// <param name="levelConfigurationId"></param>
        /// <returns></returns>
        public LevelConfigurationViewModel GetLevelConfigurationById(Guid levelConfigurationId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameters = new { levelConfigurationId };
                var sql = "select * from levelConfiguration where levelConfigurationid = @levelConfigurationId";

                var levelConfiguration = db.QuerySingle<LevelConfigurationViewModel>(sql, parameters);

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
