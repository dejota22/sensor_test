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

        [JsonProperty("ver")]
        public string ver { get; set; }

        [JsonProperty("seq")]
        public int seq { get; set; }

        [JsonProperty("resets")]
        public int resets { get; set; }

        [JsonProperty("card_lidos")]
        public int card_lidos { get; set; }

        [JsonProperty("card_send")]
        public int card_send { get; set; }

        [JsonProperty("relat_send")]
        public int RelatSend { get; set; }

        [JsonProperty("relat_erros")]
        public int relat_erros { get; set; }

        [JsonProperty("setup_bf")]
        public List<SetupBf> setup_bf { get; set; }

        [JsonProperty("setup_af")]
        public List<SetupAf> setup_af { get; set; }

        [JsonProperty("freq")]
        public double freq { get; set; }

        [JsonProperty("temp")]
        public double temp { get; set; }

        [JsonProperty("alrm")]
        public int alrm { get; set; }

        //[JsonProperty("rms_bf_acc_X")]
        //public double RmsBfAccX { get; set; }

        //[JsonProperty("rms_bf_acc_Y")]
        //public double RmsBfAccY { get; set; }

        //[JsonProperty("rms_bf_acc_Z")]
        //public double RmsBfAccZ { get; set; }

        //[JsonProperty("rms_bf_spd_X")]
        //public double RmsBfSpdX { get; set; }

        //[JsonProperty("rms_bf_spd_Y")]
        //public double RmsBfSpdY { get; set; }

        //[JsonProperty("rms_bf_spd_Z")]
        //public double RmsBfSpdZ { get; set; }

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

    public class SetupAf
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

    public class SetupBf
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