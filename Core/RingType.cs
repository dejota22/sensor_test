using System;
using System.Collections.Generic;

namespace Core
{
    public partial class RingType
    {
        public RingType()
        {
            Ring = new HashSet<Ring>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Ring> Ring { get; set; }
    }
}
