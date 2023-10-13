namespace ecommerceApi.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }

    }
}
