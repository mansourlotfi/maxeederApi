namespace ecommerceApi.Entities
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string PictureUrl { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }
        public int CategoryId { get; set; } 
        public Category Category { get; set; }
        public List<Product>? Product { get; set; } = new List<Product>();
    }
}
