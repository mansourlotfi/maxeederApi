namespace ecommerceApi.Entities
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Description { get; set; }
        public List<ProductFeature>? Products { get; set; } = new List<ProductFeature>();
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
    }
}
