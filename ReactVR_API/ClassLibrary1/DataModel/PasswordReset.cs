using System;
using System.Collections.Generic;

namespace ClassLibrary1.DataModel
{
    public partial class PasswordReset
    {
        public Guid PasswordResetId { get; set; }
        public Guid UserAccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDeleted { get; set; }

        public virtual UserAccount UserAccount { get; set; }
    }
}
