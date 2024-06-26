﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public partial class DeviceConfiguration
    {
        public DeviceConfiguration()
        {
            DeviceConfigurationHorariosEnviosCard = new HashSet<DeviceConfigurationHorariosEnviosCard>();
        }

        public int Id { get; set; }
        public int? MotorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DeviceId { get; set; }

        public int? acc_odr { get; set; }
        public int? acc_freq_cut { get; set; }
        public int? acc_filtro { get; set; }
        public int? acc_eixo { get; set; }
        public int? acc_fs { get; set; }
        public int? acc_amostras { get; set; }

        public int? spd_odr { get; set; }
        public int? spd_freq_cut { get; set; }
        public int? spd_filtro { get; set; }
        public int? spd_eixo { get; set; }
        public int? spd_fs { get; set; }
        public int? spd_amostras { get; set; }

        public int? modo_hora { get; set; }
        public int? conta_envios { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string dias_run { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string inicio_turno { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string fim_turno { get; set; }
        public int? intervalo_analise { get; set; }
        public int? intervalo_analise_alarme { get; set; }
        public int? quant_alarme { get; set; }
        public int? quant_horarios_cards { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string dia_envio_relat { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string hora_envio_relat { get; set; }
        public int? t_card_normal { get; set; }

        #region Limites
        public decimal? rms_acc_red { get; set; }
        public decimal? rms_acc_yel { get; set; }
        public decimal? min_rms_acc { get; set; }
        public decimal? rms_spd_red { get; set; }
        public decimal? rms_spd_yel { get; set; }
        public int? max_var { get; set; }
        #endregion

        public bool? config { get; set; }
        public DateTime? sent_date { get; set; }

        [ForeignKey("MotorId")]
        public Motor Motor { get; set; }

        public virtual ICollection<DeviceConfigurationHorariosEnviosCard> DeviceConfigurationHorariosEnviosCard { get; set; }
    }
}
