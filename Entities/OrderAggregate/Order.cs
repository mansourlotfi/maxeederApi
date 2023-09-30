using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.Entities.OrderAggregate
{
    public class Order
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }

        [Required]
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<OrderItem> OrderItems { get; set; }
        public long Subtotal { get; set; }
        public long DeliveryFee { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string Order_id { get; set; }
        public int? Ref { get; set; }

        public long GetTotal()
        {
            return Subtotal + DeliveryFee;
        }
    }
}
