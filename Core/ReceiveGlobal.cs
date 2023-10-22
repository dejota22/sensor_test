using Core.ApiModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public partial class ReceiveGlobal
    {
        [Key]
        public int IdReceiveGlobal { get; set; }
        public DateTime DataReceive { get; set; }
        public int? IdDeviceConfiguration { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string id { get; set; }
        [Column(TypeName = "varchar(45)")]
        public string gtw { get; set; }
        [Column(TypeName = "varchar(45)")]
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
        public double rms_acc_X { get; set; }
        public double rms_acc_Y { get; set; }
        public double rms_acc_Z { get; set; }
        public double rms_spd_X { get; set; }
        public double rms_spd_Y { get; set; }
        public double rms_spd_Z { get; set; }
        public double ftr_crista_X { get; set; }
        public double ftr_crista_Y { get; set; }
        public double ftr_crista_Z { get; set; }
        public bool? enviado_vinkins { get; private set; }

        [ForeignKey("IdDeviceConfiguration")]
        public DeviceConfiguration DeviceConfiguration { get; set; }
        
    }
}
