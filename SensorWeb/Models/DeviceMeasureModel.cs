using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SensorWeb.Models
{
    public class DeviceMeasureModel : BaseModel
    {
        public int Id { get; set; }
        [Display(Name = "Empresa")]
        public int? CompanyId { get; set; }
        [Display(Name = "Equipamento")]
        public int? MotorId { get; set; }
        public DateTime CreatedAt { get; set; }
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
        public int? VelocityMin { get; set; }
        public int? VelocityMax { get; set; }
        public int? AccelerationMin { get; set; }
        public int? AccelerationMax { get; set; }
        public int? CrestFactorMin { get; set; }
        public int? CrestFactorMax { get; set; }
        public bool Sent { get; set; }

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

        public int? usr_odr { get; set; }
        public int? usr_freq_cut { get; set; }
        public int? usr_filtro { get; set; }
        public int? usr_eixo { get; set; }
        public int? usr_fs { get; set; }
        public int? usr_amostras { get; set; }

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
    }
}
