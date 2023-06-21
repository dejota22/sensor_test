using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.ApiModel.Request
{
    public class DeviceGlobalRequest
    {
        [JsonProperty("gtw")]
        public string Gtw { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ver")]
        public string Ver { get; set; }

        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("resets")]
        public int Resets { get; set; }

        [JsonProperty("card_lidos")]
        public int CardLidos { get; set; }

        [JsonProperty("card_send")]
        public int CardSend { get; set; }

        [JsonProperty("relat_send")]
        public int RelatSend { get; set; }

        [JsonProperty("relat_erros")]
        public int RelatErros { get; set; }

        [JsonProperty("setup_bf")]
        public List<SetupBf> SetupBf { get; set; }

        [JsonProperty("setup_af")]
        public List<SetupAf> SetupAf { get; set; }

        [JsonProperty("freq")]
        public double Freq { get; set; }

        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("alrm")]
        public int Alrm { get; set; }

        [JsonProperty("rms_bf_acc_X")]
        public double RmsBfAccX { get; set; }

        [JsonProperty("rms_bf_acc_Y")]
        public double RmsBfAccY { get; set; }

        [JsonProperty("rms_bf_acc_Z")]
        public double RmsBfAccZ { get; set; }

        [JsonProperty("rms_bf_spd_X")]
        public double RmsBfSpdX { get; set; }

        [JsonProperty("rms_bf_spd_Y")]
        public double RmsBfSpdY { get; set; }

        [JsonProperty("rms_bf_spd_Z")]
        public double RmsBfSpdZ { get; set; }

        [JsonProperty("rms_af_acc_X")]
        public double RmsAfAccX { get; set; }

        [JsonProperty("rms_af_acc_Y")]
        public double RmsAfAccY { get; set; }

        [JsonProperty("rms_af_acc_Z")]
        public double RmsAfAccZ { get; set; }

        [JsonProperty("rms_af_spd_X")]
        public double RmsAfSpdX { get; set; }

        [JsonProperty("rms_af_spd_Y")]
        public double RmsAfSpdY { get; set; }

        [JsonProperty("rms_af_spd_Z")]
        public double RmsAfSpdZ { get; set; }
    }

    public class SetupAf
    {
        [JsonProperty("odr")]
        public int Odr { get; set; }

        [JsonProperty("freq_cut")]
        public int FreqCut { get; set; }

        [JsonProperty("filtro")]
        public int Filtro { get; set; }

        [JsonProperty("fs")]
        public int Fs { get; set; }

        [JsonProperty("amostras")]
        public int Amostras { get; set; }
    }

    public class SetupBf
    {
        [JsonProperty("odr")]
        public int Odr { get; set; }

        [JsonProperty("freq_cut")]
        public int FreqCut { get; set; }

        [JsonProperty("filtro")]
        public int Filtro { get; set; }

        [JsonProperty("fs")]
        public int Fs { get; set; }

        [JsonProperty("amostras")]
        public int Amostras { get; set; }
    }
}