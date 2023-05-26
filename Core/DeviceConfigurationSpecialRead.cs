using System;
using System.Collections.Generic;

namespace Core
{
    public partial class DeviceConfigurationSpecialRead
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? MotorId { get; set; }
        public int? DeviceId { get; set; }
        public bool Sent { get; set; }
        public DateTime SentDate { get; set; }

        public int? usr_odr { get; set; }
        public int? usr_freq_cut { get; set; }
        public int? usr_filtro { get; set; }
        public int? usr_eixo { get; set; }
        public int? usr_fs { get; set; }
        public int? usr_amostras { get; set; }
    }
}
