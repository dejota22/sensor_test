using System;
using System.Collections.Generic;

namespace Core
{
    public partial class CompanyAlertContact
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public virtual Company Company { get; set; }
    }
}
