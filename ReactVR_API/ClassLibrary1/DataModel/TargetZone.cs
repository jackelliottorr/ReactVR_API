using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class TargetZone
    {
        public TargetZone()
        {
            LevelConfiguration = new HashSet<LevelConfiguration>();
        }

        public Guid TargetZoneId { get; set; }
        public string TargetZoneShape { get; set; }
        public decimal TargetZoneX { get; set; }
        public decimal TargetZoneY { get; set; }
        public decimal TargetZoneZ { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<LevelConfiguration> LevelConfiguration { get; set; }
    }
}
