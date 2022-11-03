using System;
using System.Collections.Generic;

namespace Core
{
    public partial class PumpType
    {
        public PumpType()
        {
            Pump = new HashSet<Pump>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Pump> Pump { get; set; }
    }
}
