using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactVR_API.Models
{
    public class UserAccount
    {
        public Guid UserAccountId { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Salt { get; set; }

        public string Hash { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }


    public class UserAccountCreateModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }


    public class UserAccountUpdateModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
