using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class UserAccount
    {
        public UserAccount()
        {
            LevelConfiguration = new HashSet<LevelConfiguration>();
            OrganisationInvite = new HashSet<OrganisationInvite>();
            OrganisationMembership = new HashSet<OrganisationMembership>();
            PasswordReset = new HashSet<PasswordReset>();
            Scoreboard = new HashSet<Scoreboard>();
        }

        public Guid UserAccountId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<LevelConfiguration> LevelConfiguration { get; set; }
        public virtual ICollection<OrganisationInvite> OrganisationInvite { get; set; }
        public virtual ICollection<OrganisationMembership> OrganisationMembership { get; set; }
        public virtual ICollection<PasswordReset> PasswordReset { get; set; }
        public virtual ICollection<Scoreboard> Scoreboard { get; set; }
    }
}
