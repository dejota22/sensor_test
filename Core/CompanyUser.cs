using System;
using System.Collections.Generic;

namespace Core
{
    public partial class CompanyUser
    {
        public int Id { get; set; }
        public int? IdCompany { get; set; }
        public int? IdUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
