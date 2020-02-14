using System;
using System.Collections.Generic;
using ReactVR_API.Common.Enums;

namespace ReactVR_API.Common.Models
{
    public class OrganisationInvite
    {
        public Guid OrganisationInviteId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid InvitedById { get; set; }
        public Guid InviteeId { get; set; }
        public UserType InviteUserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OrganisationInviteCreateModel
    {
        public Guid OrganisationInviteId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid InvitedById { get; set; }
        //public Guid? InviteeId { get; set; }
        public string InviteeEmailAddress { get; set; }
        public UserType InviteUserType { get; set; }
    }
}
