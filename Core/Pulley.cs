using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Pulley
    {
        public Pulley()
        {
            Compressor = new HashSet<Compressor>();
            ExhaustFan = new HashSet<ExhaustFan>();
            Pump = new HashSet<Pump>();
            Reducer = new HashSet<Reducer>();
        }

        public int Id { get; set; }
        public int PulleyTypeId { get; set; }
        public double? D1 { get; set; }
        public double? D2 { get; set; }
        public double? BetweenAxles { get; set; }
        public int? BeltQuantity { get; set; }
        public double? Ratio { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }

        public virtual PulleyType PulleyType { get; set; }
        public virtual ICollection<Compressor> Compressor { get; set; }
        public virtual ICollection<ExhaustFan> ExhaustFan { get; set; }
        public virtual ICollection<Pump> Pump { get; set; }
        public virtual ICollection<Reducer> Reducer { get; set; }
    }
}
