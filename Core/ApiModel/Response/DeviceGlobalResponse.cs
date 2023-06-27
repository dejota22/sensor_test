using Core.ApiModel.Request;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.ApiModel.Response
{
    public class DeviceGlobalResponse
    {
        [JsonProperty("sensor")]
        public List<Sensor> Sensor { get; set; }

        [JsonProperty("horarios")]
        public List<Horario> Horarios { get; set; }

        [JsonProperty("gatilhos")]
        public List<Gatilho> Gatilhos { get; set; }

        [JsonProperty("lora")]
        public List<Lora> Lora { get; set; }

        [JsonProperty("versao")]
        public string Versao { get; set; }

        [JsonProperty("sn")]
        public string Sn { get; set; }
    }

    public class Gatilho
    {
        [JsonProperty("config")]
        public int Config { get; set; }

        [JsonProperty("rms_acc_red")]
        public double RmsAccRed { get; set; }

        [JsonProperty("rms_acc_yel")]
        public double RmsAccYel { get; set; }

        [JsonProperty("min_rms_acc")]
        public double MinRmsAcc { get; set; }

        [JsonProperty("rms_spd_red")]
        public double RmsSpdRed { get; set; }

        [JsonProperty("rms_spd_yel")]
        public double RmsSpdYel { get; set; }

        [JsonProperty("max_var")]
        public int MaxVar { get; set; }
    }

    public class Horario
    {
        [JsonProperty("config")]
        public int Config { get; set; }

        [JsonProperty("modo_hora")]
        public int ModoHora { get; set; }

        [JsonProperty("conta_envios")]
        public int ContaEnvios { get; set; }

        [JsonProperty("dias_run")]
        public string DiasRun { get; set; }

        [JsonProperty("inicio_turno")]
        public string InicioTurno { get; set; }

        [JsonProperty("fim_turno")]
        public string FimTurno { get; set; }

        [JsonProperty("intervalo_analise")]
        public int IntervaloAnalise { get; set; }

        [JsonProperty("intervalo_analise_alarme")]
        public int IntervaloAnaliseAlarme { get; set; }

        [JsonProperty("quant_alarme")]
        public int QuantAlarme { get; set; }

        [JsonProperty("quant_horarios_cards")]
        public int QuantHorariosCards { get; set; }

        [JsonProperty("horarios_envios_card")]
        public List<HorariosEnviosCard> HorariosEnviosCard { get; set; }

        [JsonProperty("dia_envio_relat")]
        public string DiaEnvioRelat { get; set; }

        [JsonProperty("hora_envio_relat")]
        public string HoraEnvioRelat { get; set; }
    }

    public class HorariosEnviosCard
    {
        [JsonProperty("hora")]
        public string Hora { get; set; }
    }

    public class Lora
    {
        [JsonProperty("config")]
        public int Config { get; set; }

        [JsonProperty("canal")]
        public int Canal { get; set; }

        [JsonProperty("sf")]
        public int Sf { get; set; }

        [JsonProperty("bw")]
        public int Bw { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("gtw")]
        public int Gtw { get; set; }

        [JsonProperty("syn")]
        public int Syn { get; set; }
    }

    public class Sensor
    {
        [JsonProperty("config")]
        public int Config { get; set; }

        [JsonProperty("setup_acc")]
        public List<Setup> SetupAcc { get; set; }

        [JsonProperty("setup_spd")]
        public List<Setup> SetupSpd { get; set; }

        [JsonProperty("setup_usr")]
        public List<Setup> SetupUsr { get; set; }
    }
}

