using System;
using System.Collections.Generic;

namespace ReactVR_API.Models
{
    public class Target
    {        
        public Guid TargetId { get; set; }
        public Guid LevelConfigurationId { get; set; }
        public string TargetShape { get; set; }
        public decimal TargetX { get; set; }
        public decimal TargetY { get; set; }
        public decimal TargetZ { get; set; }
        public decimal OffsetX { get; set; }
        public decimal OffsetY { get; set; }
        public decimal OffsetZ { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
