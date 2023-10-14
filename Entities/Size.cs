namespace ecommerceApi.Entities
{
    public class Size
    {
        public int Id { get; set; }
        public string SizeName { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? SizeNameEn { get; set; }
    }
}
