using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


/// <summary>
/// Classe de recebimento do request vindo do dispoitivo IoT
/// </summary>
namespace Core.ApiModel.Request
{

    public class DeviceRequest
    {
        #region Ex Aplicação Request Device

        //Comunicação do dispositivo remoto para o servidor
        //{
        //   "T": "13b579",
        //   "E": "0015",
        //   "R": "1x1y1z",
        //   "C": "1a05d71a05d71a05d7",
        //   "D": [
        //      { "1x": "80818280838480...8085" },
        //      { "1y": "80808180838485...8081" },
        //      { "1z": "81818283838481...8280" }
        //   ]
        //}

        //Sendo:
        //T - Tag do Dispositivo Remoto
        //E - Temperatura
        //R - Leituras executadas
        //C - Configurações da leitura
        //D - Dados úteis coletados pelo dispositivo remoto

        /// <summary>
        /// T - Tag do Dispositivo Remoto
        /// </summary>
        [JsonProperty("T")]
        public string T { get; set; }


        /// <summary>
        /// E - Temperatura
        /// </summary>
        [JsonProperty("E")]
        public string E { get; set; }

        /// <summary>
      //R - Leituras executadas
        /// </summary>
        [JsonProperty("R")]
        public string R { get; set; }

        /// <summary>
         //C - Configurações da leitura
        /// </summary>
        [JsonProperty("C")]
        public string C { get; set; }

        /// <summary>
        //D - Dados úteis coletados pelo dispositivo remoto
        /// </summary>        

        [JsonProperty("D")]
        //public DeviceRequestListAxle D { get; set; }
        public DeviceRequestListAxle[] D { get; set; }

        #endregion

    }

    public partial class DeviceRequestListAxle
    {
        [JsonProperty("x", NullValueHandling = NullValueHandling.Ignore)]
        public string x { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public string y { get; set; }

        [JsonProperty("z", NullValueHandling = NullValueHandling.Ignore)]
        public string z { get; set; }
    }


}



