using System;
using System.Collections.Generic;

namespace Core
{
    public partial class CompanySub
    {
        public int Id { get; set; }
        public int? IdCompany { get; set; }
        public int? IdCompanySub { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
