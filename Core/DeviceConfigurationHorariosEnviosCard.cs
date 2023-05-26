using System;
using System.Collections.Generic;

namespace Core
{
    public partial class DeviceConfigurationHorariosEnviosCard
    {
        public int Id { get; set; }
        public int DeviceConfigurationId { get; set; }
        public string Hora { get; set; }

        public virtual DeviceConfiguration DeviceConfiguration { get; set; }
    }
}
