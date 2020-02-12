using System;
using System.Collections.Generic;

namespace ReactVR_API.Common.Models
{
    public class Organisation
    {
        public Guid OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OrganisationCreateModel
    {
        public string OrganisationName { get; set; }
    }
}
