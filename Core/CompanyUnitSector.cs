using System;
using System.Collections.Generic;

namespace Core
{
    public partial class CompanyUnitSector
    {
        public CompanyUnitSector()
        {
            Machine = new HashSet<Machine>();
        }

        public int Id { get; set; }
        public int CompanyUnitId { get; set; }
        public int? ParentSectorId { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual CompanyUnit CompanyUnit { get; set; }
        public virtual CompanyUnitSector ParentSector { get; set; }

        public virtual ICollection<Machine> Machine { get; set; }
        public virtual ICollection<CompanyUnitSector> SubSectors { get; set; }
    }
}
