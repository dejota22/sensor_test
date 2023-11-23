using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SensorWeb.Models
{
    public class MotorDropdownModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UnitId { get; set; }
        public int? SectorId { get; set; }
        public int? SubSectorId { get; set; }
        public bool IsGrouping { get; set; }

        public bool IsSelected { get; set; }
    }
}
