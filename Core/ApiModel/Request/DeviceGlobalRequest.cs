using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.ApiModel.Request
{
    public class DeviceGlobalRequest
    {
        [JsonProperty("gtw")]
        public string gtw { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("versao")]
        public string versao { get; set; }

        [JsonProperty("seq")]
        public int seq { get; set; }

        [JsonProperty("resets")]
        public int resets { get; set; }

        [JsonProperty("leituras_efetuadas")]
        public int leituras_efetuadas { get; set; }

        [JsonProperty("q_cards_enviados")]
        public int q_cards_enviados { get; set; }

        [JsonProperty("q_relats_enviados")]
        public int q_relats_enviados { get; set; }

        [JsonProperty("relat_erros")]
        public int relat_erros { get; set; }

        [JsonProperty("setup_acc")]
        public List<SetupAcc> setup_acc { get; set; }

        [JsonProperty("setup_spd")]
        public List<SetupSpd> setup_spd { get; set; }

        [JsonProperty("FreqFine")]
        public double FreqFine { get; set; }

        [JsonProperty("temp")]
        public double temp { get; set; }

        [JsonProperty("alarme")]
        public int alarme { get; set; }

        [JsonProperty("rms_acc_X")]
        public double rms_acc_X { get; set; }

        [JsonProperty("rms_acc_Y")]
        public double rms_acc_Y { get; set; }

        [JsonProperty("rms_acc_Z")]
        public double rms_acc_Z { get; set; }

        [JsonProperty("rms_spd_X")]
        public double rms_spd_X { get; set; }

        [JsonProperty("rms_spd_Y")]
        public double rms_spd_Y { get; set; }

        [JsonProperty("rms_spd_Z")]
        public double rms_spd_Z { get; set; }
    }

    public class SetupSpd
    {
        [JsonProperty("odr")]
        public int odr { get; set; }

        [JsonProperty("freq_cut")]
        public int freq_cut { get; set; }

        [JsonProperty("filtro")]
        public int filtro { get; set; }

        [JsonProperty("fs")]
        public int fs { get; set; }

        [JsonProperty("amostras")]
        public int amostras { get; set; }
    }

    public class SetupAcc
    {
        [JsonProperty("odr")]
        public int odr { get; set; }

        [JsonProperty("freq_cut")]
        public int freq_cut { get; set; }

        [JsonProperty("filtro")]
        public int filtro { get; set; }

        [JsonProperty("fs")]
        public int fs { get; set; }

        [JsonProperty("amostras")]
        public int amostras { get; set; }
    }
}