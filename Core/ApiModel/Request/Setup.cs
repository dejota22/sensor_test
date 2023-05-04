using Newtonsoft.Json;

namespace Core.ApiModel.Request
{
    public class Setup
    {
        [JsonProperty("odr")]
        public int Odr { get; set; }

        [JsonProperty("freq_cut")]
        public int FreqCut { get; set; }

        [JsonProperty("filtro")]
        public int Filtro { get; set; }

        [JsonProperty("eixo")]
        public int Eixo { get; set; }

        [JsonProperty("fs")]
        public int Fs { get; set; }

        [JsonProperty("amostras")]
        public int Amostras { get; set; }
    }
}
