using System;
using System.Collections.Generic;

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
        public string dias_run { get; set; }
        public string inicio_turno { get; set; }
        public string fim_turno { get; set; }
        public int? intervalo_analise { get; set; }
        public int? intervalo_analise_alarme { get; set; }
        public int? quant_alarme { get; set; }
        public int? quant_horarios_cards { get; set; }
        public string dia_envio_relat { get; set; }
        public string hora_envio_relat { get; set; }

        #region Limites
        public decimal? max_rms_red { get; set; }
        public decimal? max_rms_yel { get; set; }
        public decimal? min_rms { get; set; }
        public int? max_percent { get; set; }
        #endregion

        public virtual ICollection<DeviceConfigurationHorariosEnviosCard> DeviceConfigurationHorariosEnviosCard { get; set; }
    }
}
