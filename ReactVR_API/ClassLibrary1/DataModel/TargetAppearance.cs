using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class TargetAppearance
    {
        public Guid TargetAppearanceId { get; set; }
        public Guid TargetId { get; set; }
        public Guid ScoreboardId { get; set; }
        public decimal TargetUptime { get; set; }
        public bool WasMissed { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Scoreboard Scoreboard { get; set; }
        public virtual Target Target { get; set; }
    }
}
