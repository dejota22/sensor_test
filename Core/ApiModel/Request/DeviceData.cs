using Newtonsoft.Json;

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
        public string Setup { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("eixo")]
        public string Eixo { get; set; }

        [JsonProperty("rms_acc_y")]
        public double RmsAccY { get; set; }

        [JsonProperty("rms_spd_y")]
        public double RmsSpdY { get; set; }

        [JsonProperty("ftr_crista_y")]
        public double FtrCristaY { get; set; }

        [JsonProperty("alarme")]
        public int Alarme { get; set; }

        [JsonProperty("dado")]
        public string Dado { get; set; }

    }
}
