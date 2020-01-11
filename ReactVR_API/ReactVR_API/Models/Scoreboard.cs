using System;
using System.Collections.Generic;

namespace ReactVR_API.Models
{
    public class Scoreboard
    {
        public Guid ScoreboardId { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid LevelConfigurationId { get; set; }
        public int Score { get; set; }
        public bool IsDeleted { get; set; }
    }
}
