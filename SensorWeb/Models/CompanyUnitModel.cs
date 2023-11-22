using Core;
using Core.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SensorWeb.Models
{
    public class CompanyUnitModel : BaseModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Required")]
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public Company Company { get; set; }

        public ICollection<CompanyUnitSector> CompanyUnitSector { get; set; }
    }
}

