using Core.ApiModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public partial class ReceiveData
    {
        public ReceiveData()
        {
            ReceiveDataDados = new HashSet<ReceiveDataDado>();
        }

        [Key]
        public int IdReceiveData { get; set; }
        public DateTime DataReceive { get; set; }
        public int? IdDeviceConfiguration { get; set; }
        public string id { get; set; }
        public string gtw { get; set; }
        public int seq { get; set; }
        public double temp { get; set; }
        public double dec { get; set; }
        public int setup_odr { get; set; }
        public int setup_freq_cut { get; set; }
        public int setup_filtro { get; set; }
        public int setup_eixo { get; set; }
        public int setup_fs { get; set; }
        public int setup_amostras { get; set; }
        public int tipo { get; set; }
        public double rms_acc { get; set; }
        public double rms_spd { get; set; }
        public double ftr_crista { get; set; }
        public int alarme { get; set; }

        public bool? enviado_vinkins { get; private set; }

        public string dado { get; set; }

        [ForeignKey("IdDeviceConfiguration")]
        public DeviceConfiguration DeviceConfiguration { get; set; }

        public virtual ICollection<ReceiveDataDado> ReceiveDataDados { get; set; }
    }
}
