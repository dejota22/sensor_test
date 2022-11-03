using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SensorWeb.Models
{
    public class DeviceMeasureModel : BaseModel
    {
        public int Id { get; set; }
        public int MotorId { get; set; }
        public string Temperature { get; set; }
        public string ReadDataType { get; set; }
        public string XAxle { get; set; }
        public string YAxle { get; set; }
        public string ZAxle { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
