namespace ecommerceApi.RequestHelpers
{
    public class ProductParams : PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public string? Types { get; set; }
        public string? Brands { get; set; }
        public string? Size { get; set; }
        public string? Usage { get; set; }
        public bool? IsActive { get; set; }
        public bool? ShowPrice { get; set; }

    }
}