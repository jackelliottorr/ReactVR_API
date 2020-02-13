using ReactVR_API.Common.Enums;
using System;
using System.Collections.Generic;

namespace ReactVR_API.Common.Models
{
    public class OrganisationMembership
    {
        public Guid OrganisationMembershipId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid UserAccountId { get; set; }
        public Nullable<Guid> OrganisationInviteId { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
