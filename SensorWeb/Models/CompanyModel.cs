using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SensorWeb.Models
{
    public class CompanyModel : BaseModel
    {
        public int Id { get; set; }

        [Display(Name = "TradeName")]
        [Required(ErrorMessage = "Required")]
        public string TradeName { get; set; }

        [Display(Name = "LegalName")]
        [Required(ErrorMessage = "Required")]
        public string LegalName { get; set; }

        [Display(Name = "Cnpj")]
        [Required(ErrorMessage = "Required")]
        public string Cnpj { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Empresa Gerenciadora")]
        public int ParentCompanyId { get; set; }

        [Display(Name = "Tipo empresa")]
        [Required(ErrorMessage = "Required")]
        public string CompanyTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        [Display(Name = "UpdateDate")]
        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        [Display(Name = "Empresas Locatárias")]
        public List<SelectListItem> lstCompanySub { get; set; }

        [Display(Name = "Usuários para acesso")]
        public List<SelectListItem> lstUsers { get; set; }


        [Display(Name = "Empresas Locatárias")]
        public List<SelectListItem> lstCompanySubUnique { get; set; }

        [Display(Name = "Usuários para acesso")]
        public List<SelectListItem> lstUsersUnique { get; set; }


        [Display(Name = "Sensores")]
        public List<SelectListItem> lstSensorUnique { get; set; }
        
        [Display(Name = "Sensores")]
        public List<SelectListItem> lstSensor { get; set; }
        
        public virtual string UpdatedAtSt { get { return UpdatedAt.ToShortDateString(); } }
    }
}

