using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Dados
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime Date { get; set; }
        public string Data { get; set; }

        public virtual Device Device { get; set; }
    }
}
