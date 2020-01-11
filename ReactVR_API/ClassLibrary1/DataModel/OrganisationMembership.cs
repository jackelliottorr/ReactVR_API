using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class OrganisationMembership
    {
        public Guid OrganisationMembershipId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid UserAccountId { get; set; }
        public int UserTypeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Organisation Organisation { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
