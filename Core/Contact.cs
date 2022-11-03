using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Contact
    {
        public Contact()
        {
            InverseManagedByNavigation = new HashSet<Contact>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? ManagedBy { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Department { get; set; }
        public byte IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Company Company { get; set; }
        public virtual Contact ManagedByNavigation { get; set; }
        public virtual ICollection<Contact> InverseManagedByNavigation { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
