using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.ApiModel.Request
{
    public class DeviceDataRequest
    {
        [JsonProperty("gtw")]
        public string gtw { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("temp")]
        public double temp { get; set; }

        [JsonProperty("dec")]
        public double dec { get; set; }

        [JsonProperty("setup")]
        public List<Setup> setup { get; set; }

        [JsonProperty("tipo")]
        public int tipo { get; set; }

        [JsonProperty("rms_acc")]
        public double rms_acc { get; set; }

        [JsonProperty("rms_spd")]
        public double rms_spd { get; set; }

        [JsonProperty("ftr_crista")]
        public double ftr_crista { get; set; }

        [JsonProperty("alarme")]
        public int alarme { get; set; }

        [JsonProperty("dado")]
        public string dado { get; set; }
    }
}
