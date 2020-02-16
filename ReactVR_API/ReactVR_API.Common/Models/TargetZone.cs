using System;
using System.Collections.Generic;

namespace ReactVR_API.Common.Models
{
    public class TargetZone
    {
        public Guid TargetZoneId { get; set; }
        public Guid LevelConfigurationId { get; set; }
        public string TargetZoneShape { get; set; }
        public decimal TargetZoneX { get; set; }
        public decimal TargetZoneY { get; set; }
        public decimal TargetZoneZ { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
