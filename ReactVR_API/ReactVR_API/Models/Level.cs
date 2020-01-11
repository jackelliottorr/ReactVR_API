using System;
using System.Collections.Generic;

namespace ReactVR_API.Models
{
    public class Level
    {
        public Guid LevelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
