namespace ecommerceApi.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public string MediaFileName { get; set; }
        public int ProductId { get; set; } 
        public Product Product { get; set; } = null!; 
    }
}
