namespace ecommerceApi.Entities
{
    public class ProductFeature
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
