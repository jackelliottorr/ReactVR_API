using System;
using System.Collections.Generic;

namespace ReactVR_API.Models
{
    public class PasswordReset
    {
        public Guid PasswordResetId { get; set; }
        public Guid UserAccountId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDeleted { get; set; }
    }
}
