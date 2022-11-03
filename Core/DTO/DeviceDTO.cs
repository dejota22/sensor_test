using System;
using System.Collections.Generic;

namespace Core
{
    public partial class DeviceDTO
    {
        //public DeviceDTO()
        //{
        //    Compressor = new HashSet<Compressor>();
        //    Dados = new HashSet<Dados>();
        //    ExhaustFan = new HashSet<ExhaustFan>();
        //    Pump = new HashSet<Pump>();
        //    Reducer = new HashSet<Reducer>();
        //}

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Tag { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        //public virtual Company Company { get; set; }
        //public virtual ICollection<Compressor> Compressor { get; set; }
        //public virtual ICollection<Dados> Dados { get; set; }
        //public virtual ICollection<ExhaustFan> ExhaustFan { get; set; }
        //public virtual ICollection<Pump> Pump { get; set; }
        //public virtual ICollection<Reducer> Reducer { get; set; }
    }
}
