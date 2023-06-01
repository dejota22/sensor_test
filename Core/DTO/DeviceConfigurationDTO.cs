using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace Core.DTO
{
    public class DeviceConfigurationModel
    {
        public DeviceConfigurationModel() 
        {
            DeviceConfigurationHorariosEnviosCard = new List<DeviceConfigurationHorariosEnviosCard>();
        }

        public int Id { get; set; }

        [Display(Name = "Empresa")]
        public int? CompanyId { get; set; }

        [Display(Name = "Equipamento")]
        public int? MotorId { get; set; }

        [Display(Name = "Sensor")]
        public int? DeviceId { get; set; }

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

        public DateTime SentDate { get; set; }

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

        #region Lora
        public int? canal { get; set; }
        public int? sf { get; set; }
        public int? bw { get; set; }
        public int? end { get; set; }
        public int? gtw { get; set; }
        public int? syn { get; set; }
        #endregion

        public bool isEdit { get; set; }
        public bool isEditUserSetup { get; set; }
        public bool? config { get; set; }

        public IList<DeviceConfigurationHorariosEnviosCard> DeviceConfigurationHorariosEnviosCard { get; set; }

        public DeviceConfiguration GetDeviceConfigurationFromModel()
        {
            var deviceConfig = new DeviceConfiguration()
            {
                Id = Id,
                MotorId = MotorId,
                DeviceId = DeviceId,
                acc_amostras = acc_amostras,
                acc_eixo = acc_eixo,
                acc_filtro = acc_filtro,
                acc_freq_cut = acc_freq_cut,
                acc_fs = acc_fs,
                acc_odr = acc_odr,
                spd_amostras = spd_amostras,
                spd_eixo = spd_eixo,
                spd_filtro = spd_filtro,
                spd_freq_cut = spd_freq_cut,
                spd_fs = spd_fs,
                spd_odr = spd_odr,
                modo_hora = modo_hora,
                conta_envios = conta_envios,
                dias_run = dias_run,
                inicio_turno = inicio_turno,
                fim_turno = fim_turno,
                intervalo_analise = intervalo_analise,
                intervalo_analise_alarme = intervalo_analise_alarme,
                quant_alarme = quant_alarme,
                quant_horarios_cards = quant_horarios_cards,
                dia_envio_relat = dia_envio_relat,
                hora_envio_relat = hora_envio_relat,
                max_rms_red = max_rms_red,
                max_rms_yel = max_rms_yel,
                min_rms = min_rms,
                max_percent = max_percent,
                config = config
            };

            foreach (var hora in DeviceConfigurationHorariosEnviosCard)
            {
                if (hora.Hora != null)
                {
                    hora.DeviceConfigurationId = deviceConfig.Id;
                    deviceConfig.DeviceConfigurationHorariosEnviosCard.Add(hora);
                }
            }

            return deviceConfig;
        }

        public DeviceConfigurationModel GetModelFromEntity(DeviceConfiguration entity)
        {
            var deviceConfigModel = new DeviceConfigurationModel()
            {
                Id = entity.Id,
                MotorId = entity.MotorId,
                DeviceId = entity.DeviceId,
                CreatedAt = entity.CreatedAt,
                acc_amostras = entity.acc_amostras,
                acc_eixo = entity.acc_eixo,
                acc_filtro = entity.acc_filtro,
                acc_freq_cut = entity.acc_freq_cut,
                acc_fs = entity.acc_fs,
                acc_odr = entity.acc_odr,
                spd_amostras = entity.spd_amostras,
                spd_eixo = entity.spd_eixo,
                spd_filtro = entity.spd_filtro,
                spd_freq_cut = entity.spd_freq_cut,
                spd_fs = entity.spd_fs,
                spd_odr = entity.spd_odr,
                modo_hora = entity.modo_hora,
                conta_envios = entity.conta_envios,
                dias_run = entity.dias_run,
                inicio_turno = entity.inicio_turno,
                fim_turno = entity.fim_turno,
                intervalo_analise = entity.intervalo_analise,
                intervalo_analise_alarme = entity.intervalo_analise_alarme,
                quant_alarme = entity.quant_alarme,
                quant_horarios_cards = entity.quant_horarios_cards,
                dia_envio_relat = entity.dia_envio_relat,
                hora_envio_relat = entity.hora_envio_relat,
                max_rms_red = entity.max_rms_red,
                max_rms_yel = entity.max_rms_yel,
                min_rms = entity.min_rms,
                max_percent = entity.max_percent,
                config = entity.config
            };

            foreach (var hora in entity.DeviceConfigurationHorariosEnviosCard)
            {
                if (hora.Hora != null)
                {
                    hora.DeviceConfigurationId = deviceConfigModel.Id;
                    deviceConfigModel.DeviceConfigurationHorariosEnviosCard.Add(hora);
                }
            }

            return deviceConfigModel;
        }

        public DeviceConfigurationSpecialRead GetDeviceConfigurationSpecialReadFromModel()
        {
            return new DeviceConfigurationSpecialRead()
            {
                MotorId = MotorId,
                DeviceId = DeviceId,
                usr_amostras = usr_amostras,
                usr_eixo = usr_eixo,
                usr_filtro = usr_filtro,
                usr_freq_cut = usr_freq_cut,
                usr_fs = usr_fs,
                usr_odr = usr_odr
            };
        }
    }
}
