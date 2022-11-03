using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Coupling
    {
        public Coupling()
        {
            Compressor = new HashSet<Compressor>();
            ExhaustFan = new HashSet<ExhaustFan>();
            Pump = new HashSet<Pump>();
            Reducer = new HashSet<Reducer>();
        }

        public int Id { get; set; }
        public int CouplingTypeId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual CouplingType CouplingType { get; set; }
        public virtual ICollection<Compressor> Compressor { get; set; }
        public virtual ICollection<ExhaustFan> ExhaustFan { get; set; }
        public virtual ICollection<Pump> Pump { get; set; }
        public virtual ICollection<Reducer> Reducer { get; set; }
    }
}
