using Core.ApiModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.ApiModel.Response
{
    public class DeviceGlobalResponse
    {
        [JsonProperty("gatilhos")]
        public List<Gatilho> Gatilhos { get; set; }

        [JsonProperty("lora")]
        public List<Lora> Lora { get; set; }

        [JsonProperty("sensor")]
        public List<Sensor> Sensor { get; set; }

        [JsonProperty("tempos")]
        public List<Tempo> Tempos { get; set; }
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

        [JsonProperty("max_var")]
        public int MaxVar { get; set; }

        public static implicit operator List<object>(Gatilho v)
        {
            throw new NotImplementedException();
        }
    }

    public class Lora
    {
        [JsonProperty("config")]
        public int Config { get; set; }

        [JsonProperty("canal")]
        public int Canal { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("gtw")]
        public int Gtw { get; set; }

        [JsonProperty("skw")]
        public int Skw { get; set; }

        [JsonProperty("sf")]
        public int Sf { get; set; }

        [JsonProperty("bw")]
        public int Bw { get; set; }
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

    public class Tempo
    {
        [JsonProperty("config")]
        public int Config { get; set; }

        [JsonProperty("s_sleep")]
        public int SSleep { get; set; }

        [JsonProperty("envia_card")]
        public int EnviaCard { get; set; }
    }
}

