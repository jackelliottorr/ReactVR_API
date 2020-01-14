using System;
using System.Collections.Generic;

namespace ReactVR_API.Models
{
    public class OrganisationMembership
    {
        public Guid OrganisationMembershipId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid UserAccountId { get; set; }
        public Guid OrganisationInviteId { get; set; }
        public int UserTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
