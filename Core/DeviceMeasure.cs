using System;
using System.Collections.Generic;

namespace Core
{
    public partial class DeviceMeasure
    {
        public int Id { get; set; }
        public int MotorId { get; set; }
        public int Frequency { get; set; }
        public int Bdr { get; set; }
        public string Cutoff { get; set; }
        public int Lpf { get; set; }
        public int Lines { get; set; }
        public int Axies { get; set; }
        public string Rms { get; set; }
        public string Battery { get; set; }
        public int Hours { get; set; }
        public int Temperature { get; set; }
        public string RmsMax { get; set; }
        public string RmsMin { get; set; }
        public int VelocityMin { get; set; }
        public int VelocityMax { get; set; }
        public int AccelerationMin { get; set; }
        public int AccelerationMax { get; set; }
        public int CrestFactorMin { get; set; }
        public int CrestFactorMax { get; set; }
        public bool Sent { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DeviceId { get; set; }
    }
}
