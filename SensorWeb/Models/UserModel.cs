using Core.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SensorWeb.Models
{
    public class UserModel :BaseModel
    {
        [Display(Name = "Code")]
        [Key]
        [Required(ErrorMessage = "Required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(45, MinimumLength = 10, ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }

        [Display(Name = "Password Confirm")]
       // [Required(ErrorMessage = "Required")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "UserTypeId")]
        [Required(ErrorMessage = "Required")]
        public int UserTypeId { get; set; }

        [Display(Name = "ContactId")]
        [Required(ErrorMessage = "Required")]
        public int ContactId { get; set; }

        [Display(Name = "IsActive")]
        public byte IsActive { get; set; }
        public virtual bool IsActiveBool { get { if (IsActive == 1) return true; else return false; } }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ContactModel Contact { get; set; }

    }
}
