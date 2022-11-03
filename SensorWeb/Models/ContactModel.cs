using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SensorWeb.Models
{
    public class ContactModel : BaseModel
    {
        public int Id { get; set; }

        [Display(Name = "CompanyId")]
        [Required(ErrorMessage = "Required")]
        public int CompanyId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Required")]
        public string Surname { get; set; }

        [Display(Name = "Name")]
        public virtual string NomeCompleto
        {
            get { return FirstName + " " + Surname; }
        }

        public string Email { get; set; }
        
        [Display(Name = "Document")]
        [StringLength(11, MinimumLength = 9, ErrorMessage = "DocumentInvalid")]
        public string Cpf { get; set; }

        [Display(Name = "Rg")]
        public string Rg { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "IsActive")]
        public byte IsActive { get; set; }
        public virtual bool IsActiveBool { get { if (IsActive == 1) return true; else return false; } }

        public DateTime CreatedAt { get; set; }

        
        [Display(Name = "UpdateDate")]
        public DateTime UpdatedAt { get; set; }
    }
}
