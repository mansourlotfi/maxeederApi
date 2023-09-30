using ecommerceApi.Entities.OrderAggregate;

namespace ecommerceApi.DTOs
{
    public class CreateOrderDto
    {
        public bool SaveAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public int? Ref { get; set; }
        public bool? NoDelivery { get; set; }
    }
}
