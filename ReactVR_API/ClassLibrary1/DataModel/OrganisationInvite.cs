using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class OrganisationInvite
    {
        public Guid OrganisationInviteId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid InvitedById { get; set; }
        public int InviteUserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDeleted { get; set; }

        public virtual UserType InviteUserTypeNavigation { get; set; }
        public virtual UserAccount InvitedBy { get; set; }
        public virtual Organisation Organisation { get; set; }
    }
}
