using System;
using System.Collections.Generic;

namespace Core
{
    public partial class ConfigLog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PrimaryId { get; set; }
        public int SecondaryId { get; set; }
        public string PrimaryName { get; set; }
        public string SecondaryName { get; set; }
        public string UserName { get; set; }
        public bool IsChange { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
    }
}
