using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class UserType
    {
        public UserType()
        {
            OrganisationInvite = new HashSet<OrganisationInvite>();
            OrganisationMembership = new HashSet<OrganisationMembership>();
        }

        public int UserTypeId { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<OrganisationInvite> OrganisationInvite { get; set; }
        public virtual ICollection<OrganisationMembership> OrganisationMembership { get; set; }
    }
}
