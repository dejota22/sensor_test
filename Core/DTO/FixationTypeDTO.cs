using System;
using System.Collections.Generic;

namespace Core
{
    public partial class FixationTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Compressor> Compressor { get; set; }
        public virtual ICollection<ExhaustFan> ExhaustFan { get; set; }
        public virtual ICollection<Motor> Motor { get; set; }
        public virtual ICollection<Pump> Pump { get; set; }
        public virtual ICollection<Reducer> Reducer { get; set; }

    }
}
