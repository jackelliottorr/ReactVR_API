using System;
using System.Collections.Generic;

namespace ReactVR_CORE.Models
{
    public class Organisation
    {
        public Guid OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
