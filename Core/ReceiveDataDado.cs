using Core.ApiModel.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public partial class ReceiveDataDado
    {
        [Key]
        public int IdReceiveDataDado { get; set; }
        public int IdReceiveData { get; set; }

        public int seq { get; set; }
        public double valor { get; set; }
        public double tempo { get; set; }

        public virtual ReceiveData ReceiveData { get; set; }
    }
}
