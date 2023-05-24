using System;
using System.Collections.Generic;

namespace Core
{
    public partial class DeviceMeasureHorariosEnviosCard
    {
        public int Id { get; set; }
        public int DeviceMeasureId { get; set; }
        public string Hora { get; set; }

        //public virtual DeviceMeasure DeviceMeasure { get; set; }
    }
}
