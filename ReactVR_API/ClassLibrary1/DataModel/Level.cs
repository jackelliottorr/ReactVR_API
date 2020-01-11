using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class Level
    {
        public Level()
        {
            LevelConfiguration = new HashSet<LevelConfiguration>();
        }

        public Guid LevelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<LevelConfiguration> LevelConfiguration { get; set; }
    }
}
