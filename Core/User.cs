using System;
using System.Collections.Generic;

namespace Core
{
    public partial class User
    {
        public int Id { get; set; }
        public int UserTypeId { get; set; }
        public int ContactId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
