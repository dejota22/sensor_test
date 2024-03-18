namespace Core.DTO
{
    public class SelectListCustomItemDTO
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public int? CompanyId { get; set; }
        public int? Unit { get; set; }
        public int? Sector { get; set; }
        public int? SubSector { get; set; }
    }
}
