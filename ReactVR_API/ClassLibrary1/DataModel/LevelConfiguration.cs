using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class LevelConfiguration
    {
        public LevelConfiguration()
        {
            Scoreboard = new HashSet<Scoreboard>();
            Target = new HashSet<Target>();
        }

        public Guid LevelConfigurationId { get; set; }
        public Guid LevelId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid CreatedById { get; set; }
        public Guid TargetZoneId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? TargetSpawnDelay { get; set; }
        public decimal? TargetLifespan { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }

        public virtual UserAccount CreatedBy { get; set; }
        public virtual Level Level { get; set; }
        public virtual Organisation Organisation { get; set; }
        public virtual TargetZone TargetZone { get; set; }
        public virtual ICollection<Scoreboard> Scoreboard { get; set; }
        public virtual ICollection<Target> Target { get; set; }
    }
}
