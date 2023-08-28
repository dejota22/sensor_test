using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public string TradeName { get; set; }
        public string LegalName { get; set; }
        public string Cnpj { get; set; }
        public string Website { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<CompanyUnit> CompanyUnit { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<Device> Device { get; set; }
        public virtual ICollection<CompanyAlertContact> CompanyAlertContact { get; set; }
    }
}
