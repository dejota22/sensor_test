using System;
using System.Collections.Generic;

namespace Core
{
    public partial class Company
    {
        public Company()
        {
            CompanyUnit = new HashSet<CompanyUnit>();
            Contact = new HashSet<Contact>();
            Device = new HashSet<Device>();
            CompanyAlertContact = new HashSet<CompanyAlertContact>();
        }
        public int Id { get; set; }
        public string TradeName { get; set; }
        public string LegalName { get; set; }
        public string Cnpj { get; set; }
        public string Website { get; set; }
        public string Country { get; set; }
        public int ParentCompanyId { get; set; }
        public int CompanyTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeviceMotorMaxChanges { get; set; }
        public string VikingsSendDataTime { get; set; }
        public virtual CompanyType CompanyType { get; set; }
        public virtual Company ParentCompany { get; set; }
        public virtual ICollection<Company> ParentCompanyCol { get; set; }
        public virtual ICollection<CompanyUnit> CompanyUnit { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<Device> Device { get; set; }

        public virtual ICollection<Motor> Motors { get; set; }

        public virtual ICollection<CompanyAlertContact> CompanyAlertContact { get; set; }
    }
}
