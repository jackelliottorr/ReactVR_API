using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class Organisation
    {
        public Organisation()
        {
            LevelConfiguration = new HashSet<LevelConfiguration>();
            OrganisationInvite = new HashSet<OrganisationInvite>();
            OrganisationMembership = new HashSet<OrganisationMembership>();
        }

        public Guid OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<LevelConfiguration> LevelConfiguration { get; set; }
        public virtual ICollection<OrganisationInvite> OrganisationInvite { get; set; }
        public virtual ICollection<OrganisationMembership> OrganisationMembership { get; set; }
    }
}
