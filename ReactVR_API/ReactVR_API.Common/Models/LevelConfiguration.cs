using System;
using System.Collections.Generic;

namespace ReactVR_API.Common.Models
{
    public class LevelConfiguration
    {
        public Guid LevelConfigurationId { get; set; }
        public Guid LevelId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid CreatedById { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? TargetSpawnDelay { get; set; }
        public decimal? TargetLifespan { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class LevelConfigurationViewModel
    {
        public Guid LevelConfigurationId { get; set; }
        public Guid LevelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? TargetSpawnDelay { get; set; }
        public decimal? TargetLifespan { get; set; }
        public TargetZone TargetZone { get; set; }
        public List<Target> Targets { get; set; }
    }
}
