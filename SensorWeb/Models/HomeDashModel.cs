using Core;
using Core.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SensorWeb.Models
{
    public class HomeDashModel : BaseModel
    {
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int? UnitId { get; set; }
        public string UnitName { get; set; }

        public int? SectorId { get; set; }
        public string SectorName { get; set; }

        public int? SubSectorId { get; set; }
        public string SubSectorName { get; set; }

        public int? MotorId { get; set; }
        public string MotorName { get; set; }

        public int? DeviceId { get; set; }

        public string AlertStatus { get; set; }

        public int AlertQtd { get; set; }
    }
}

