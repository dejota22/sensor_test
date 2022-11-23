using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SensorWeb.Models
{
    public class DeviceModel : BaseModel
    {
        public int Id { get; set; }

        [Display(Name = "CompanyId")]
        public int CompanyId { get; set; }

        [Display(Name = "Tag")]
        public string Tag { get; set; }
        public byte[] QrCodeImg { get; set; }


        [Display(Name = "Code")]
        public string Code { get; set; }                

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int? Frequency { get; set; }
        public int? Bdr { get; set; }
        public string Cutoff { get; set; }
        public int? Lpf { get; set; }
        public int? Lines { get; set; }
        public int? Axies { get; set; }
        public string Rms { get; set; }
        public string Battery { get; set; }
        public int? Hours { get; set; }
        public int? Temperature { get; set; }
        public string RmsMax { get; set; }
        public string RmsMin { get; set; }

        public virtual Company Company { get; set; }
        //public virtual ICollection<Compressor> Compressor { get; set; }
        //public virtual ICollection<Dados> Dados { get; set; }
        //public virtual ICollection<ExhaustFan> ExhaustFan { get; set; }
        //public virtual ICollection<Pump> Pump { get; set; }
        //public virtual ICollection<Reducer> Reducer { get; set; }


    }
}
