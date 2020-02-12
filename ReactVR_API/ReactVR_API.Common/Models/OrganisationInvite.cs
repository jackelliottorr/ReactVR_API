using System;
using System.Collections.Generic;

namespace ReactVR_API.Common.Models
{
    public class OrganisationInvite
    {
        public Guid OrganisationInviteId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid InvitedById { get; set; }
        public int InviteUserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDeleted { get; set; }
    }
}
