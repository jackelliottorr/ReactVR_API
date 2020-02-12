using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Common.Enums
{
    // don't think the db table is needed for this to be honest
    public enum UserType
    {
        SystemAdmin = 0,
        OrganisationOwner = 1,
        OrganisationAdmin = 2,
        OrganisationCoach = 3,
        OrganisationPlayer = 4
    }
}
