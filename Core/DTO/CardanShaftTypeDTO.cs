using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class CardanShaftTypeDTO
    {
        public CardanShaftTypeDTO()
        {
            Compressor = new HashSet<Compressor>();
            ExhaustFan = new HashSet<ExhaustFan>();
            Motor = new HashSet<Motor>();
            Pump = new HashSet<Pump>();
            Reducer = new HashSet<Reducer>();
        }

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
