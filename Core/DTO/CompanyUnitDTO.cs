using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class CompanyUnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<CompanyUnitSector> CompanyUnitSector { get; set; }
    }
}
