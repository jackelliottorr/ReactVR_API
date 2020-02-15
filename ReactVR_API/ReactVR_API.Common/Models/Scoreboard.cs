using System;
using System.Collections.Generic;

namespace ReactVR_API.Common.Models
{
    public class Scoreboard
    {
        public Guid ScoreboardId { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid LevelConfigurationId { get; set; }
        public int Score { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ScoreboardCreateModel
    {
        public Guid LevelConfigurationId { get; set; }
        public int Score { get; set; }
    }

    public class ScoreboardViewModel
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
