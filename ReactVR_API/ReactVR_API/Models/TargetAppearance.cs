using System;
using System.Collections.Generic;

namespace ReactVR_API.Models
{
    public class TargetAppearance
    {
        public Guid TargetAppearanceId { get; set; }
        public Guid TargetId { get; set; }
        public Guid ScoreboardId { get; set; }
        public decimal TargetUptime { get; set; }
        public bool WasMissed { get; set; }
        public bool IsDeleted { get; set; }
    }
}
