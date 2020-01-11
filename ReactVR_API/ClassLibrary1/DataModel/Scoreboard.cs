using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class Scoreboard
    {
        public Scoreboard()
        {
            TargetAppearance = new HashSet<TargetAppearance>();
        }

        public Guid ScoreboardId { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid LevelConfigurationId { get; set; }
        public int Score { get; set; }
        public bool IsDeleted { get; set; }

        public virtual LevelConfiguration LevelConfiguration { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<TargetAppearance> TargetAppearance { get; set; }
    }
}
