using System;
using System.Collections.Generic;

namespace Core
{
    public partial class CouplingType
    {
        public CouplingType()
        {
            Coupling = new HashSet<Coupling>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Coupling> Coupling { get; set; }
    }
}
