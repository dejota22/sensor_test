using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SensorWeb.Models
{
    public class UserTypeModel : BaseModel
    {
        [Display(Name = "Code")]
        [Key]
        [Required(ErrorMessage = "Required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Invalid Name")]

        [Display(Name = "Type")]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        [Display(Name = "UpdateDate")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "UpdateDate")]

        public virtual string UpdatedAtSt { get { return UpdatedAt.ToShortDateString(); } }

    }
}
