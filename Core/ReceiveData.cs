using Core.ApiModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core
{
    public partial class ReceiveData
    {
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
        public string dado { get; set; }
    }
}
