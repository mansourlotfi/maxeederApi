namespace ecommerceApi.Entities
{
 
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? Price { get; set; }
        public string PictureUrl { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int QuantityInStock { get; set; }
        public bool? IsFeatured { get; set; } = false;
        public List<ProductFeature>? Features { get; set; } = new List<ProductFeature>();
        public bool? IsActive { get; set; } = true;
        public string? Usage { get; set; }
        public string? Size { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        public List<Media>? MediaList { get; set; } = new List<Media>();

    }
}

