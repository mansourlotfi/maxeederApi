namespace ecommerceApi.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }
    }
}
