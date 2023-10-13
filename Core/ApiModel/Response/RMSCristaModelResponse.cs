using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ApiModel.Response
{
    public class RMSCristaModelResponse
    {
        public long DataReceive { get; set; }
        public double Value { get; set; }
        public string Origem { get; set; }
        public int? DataDevice { get; set; }
    }
}
