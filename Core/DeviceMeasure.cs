using System;
using System.Collections.Generic;

namespace Core
{
    public partial class DeviceMeasure
    {
        public int Id { get; set; }
        public int MotorId { get; set; }
        public string Temperature { get; set; }
        public string ReadDataType { get; set; }
        public string XAxle { get; set; }
        public string YAxle { get; set; }
        public string ZAxle { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DeviceId { get; set; }
    }
}
