using Newtonsoft.Json;

namespace Core.ApiModel.Request
{
    public class Setup
    {
        [JsonProperty("odr")]
        public int Odr { get; set; }

        [JsonProperty("freq_cut")]
        public int freq_cut { get; set; }

        [JsonProperty("filtro")]
        public int filtro { get; set; }

        [JsonProperty("eixo")]
        public int eixo { get; set; }

        [JsonProperty("fs")]
        public int fs { get; set; }

        [JsonProperty("amostras")]
        public int amostras { get; set; }
    }
}
