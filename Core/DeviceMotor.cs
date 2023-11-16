using System;
using System.Collections.Generic;

namespace Core
{
    public partial class DeviceMotor
    {
        public int Id { get; set; }
        public int MotorId { get; set; }
        public string MeasurementPlan { get; set; }
        public string SensorOrientation { get; set; }

        public virtual Motor Motor { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
