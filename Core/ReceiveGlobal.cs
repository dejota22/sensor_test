using Core.ApiModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core
{
    public partial class ReceiveGlobal
    {
        public int IdReceiveGlobal { get; set; }
        public DateTime DataReceive { get; set; }
        public int? IdDeviceConfiguration { get; set; }

        public string id { get; set; }
        public string gtw { get; set; }
        public string ver { get; set; }
        public int seq { get; set; }
        public int resets { get; set; }
        public int card_lidos { get; set; }
        public int card_send { get; set; }
        public int relat_send { get; set; }
        public int relat_erros { get; set; }
        public int setup_bf_odr { get; set; }
        public int setup_bf_freq_cut { get; set; }
        public int setup_bf_filtro { get; set; }
        public int setup_bf_fs { get; set; }
        public int setup_bf_amostras { get; set; }
        public int setup_af_odr { get; set; }
        public int setup_af_freq_cut { get; set; }
        public int setup_af_filtro { get; set; }
        public int setup_af_fs { get; set; }
        public int setup_af_amostras { get; set; }
        public double freq { get; set; }
        public double temp { get; set; }
        public int alrm { get; set; }
        public double rms_bf_acc_X { get; set; }
        public double rms_bf_acc_Y { get; set; }
        public double rms_bf_acc_Z { get; set; }
        public double rms_bf_spd_X { get; set; }
        public double rms_bf_spd_Y { get; set; }
        public double rms_bf_spd_Z { get; set; }
        public double rms_af_acc_X { get; set; }
        public double rms_af_acc_Y { get; set; }
        public double rms_af_acc_Z { get; set; }
        public double rms_af_spd_X { get; set; }
        public double rms_af_spd_Y { get; set; }
        public double rms_af_spd_Z { get; set; }
    }
}
