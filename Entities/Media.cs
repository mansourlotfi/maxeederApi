namespace ecommerceApi.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public string MediaFileName { get; set; }
        public int ProductId { get; set; } // Required foreign key property
        public Product Product { get; set; } = null!; // Required reference navigation to principal
    }
}
