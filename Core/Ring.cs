using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Ring
    {
        public Ring()
        {
            Compressor = new HashSet<Compressor>();
            ExhaustFan = new HashSet<ExhaustFan>();
            Pump = new HashSet<Pump>();
            Reducer = new HashSet<Reducer>();
        }

        public int Id { get; set; }
        public int RingTypeId { get; set; }
        public int? DriveGearTeethZ1 { get; set; }
        public int? DriveGearTeethZ2 { get; set; }
        public string Ratio { get; set; }
        public double? BetweenAxles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual RingType RingType { get; set; }
        public virtual ICollection<Compressor> Compressor { get; set; }
        public virtual ICollection<ExhaustFan> ExhaustFan { get; set; }
        public virtual ICollection<Pump> Pump { get; set; }
        public virtual ICollection<Reducer> Reducer { get; set; }
    }
}
