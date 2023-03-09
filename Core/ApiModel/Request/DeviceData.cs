using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.ApiModel.Request
{
    public class DeviceData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("dec")]
        public double Dec { get; set; }

        [JsonProperty("setup")]
        public List<Setup> Setup { get; set; }

        [JsonProperty("tipo")]
        public int Tipo { get; set; }

        [JsonProperty("ms_acc")]
        public double RmsAcc { get; set; }

        [JsonProperty("rms_spd")]
        public double RmsSpd { get; set; }

        [JsonProperty("ftr_crista")]
        public double FtrCrista { get; set; }

        [JsonProperty("alarme")]
        public int Alarme { get; set; }

        [JsonProperty("dado")]
        public string Dado { get; set; }

    }
}
