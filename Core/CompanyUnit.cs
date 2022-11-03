using System;
using System.Collections.Generic;

namespace Core
{
    public partial class CompanyUnit
    {
        public CompanyUnit()
        {
            CompanyUnitSector = new HashSet<CompanyUnitSector>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CompanyUnitSector> CompanyUnitSector { get; set; }
    }
}
