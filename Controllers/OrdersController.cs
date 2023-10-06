using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;
using ecommerceApi.Entities.OrderAggregate;
using ecommerceApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using Newtonsoft;

namespace ecommerceApi.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name)
                .ToListAsync();

            return orders;
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .FirstOrDefaultAsync(x => x.BuyerId == User.Identity.Name && x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentObjectDto>> CreateOrder(CreateOrderDto orderDto)
        {
            var basket = await _context.Baskets
                .RetrieveBasketWithItems(User.Identity.Name)
                .FirstOrDefaultAsync();

            if (basket == null) return BadRequest(new ProblemDetails
            {
                Title = "Could not find basket"
            });

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _context.Products.FindAsync(item.ProductId);
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };
                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price??0,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 300000 ? 0 : 30000;
            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = (bool)orderDto.NoDelivery ? 0 : deliveryFee,
                Order_id = basket.Order_id ?? Guid.NewGuid().ToString("N"),
                Ref = orderDto.Ref??null

            };

            

            if (orderDto.SaveAddress)
            {
                var user = await _context.Users
                    .Include(a => a.Address)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                var address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    City = orderDto.ShippingAddress.City,
                    State = orderDto.ShippingAddress.State,
                    Zip = orderDto.ShippingAddress.Zip,
                    PhoneNumber = orderDto.ShippingAddress.PhoneNumber
                };
                user.Address = address;
            }

            var options = new RestClientOptions("https://pay.org/nx/gateway/token")
            {
                ThrowOnAnyError = true,               
            };
            var client = new RestClient(options);

            var request = new RestRequest
            {
                Timeout = -1
            };

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("api_key", "4c94e3ab-c6af-487d-99b7-760a9dcd77d9");
            request.AddParameter("amount", (subtotal + deliveryFee).ToString());
            request.AddParameter("order_id", order.Order_id);
            request.AddParameter("customer_phone", orderDto.ShippingAddress.PhoneNumber);
            request.AddParameter("payer_name", orderDto.ShippingAddress.FullName);
            request.AddParameter("callback_uri", "https://www.maxeeder.com/checkout?callback=true");
            request.AddParameter("auto_verify", "yes");

            var response = await client.PostAsync<PaymentObjectDto>(request);


            if (response != null)
            {
                if (response.Code == -1)
                {

                    _context.Orders.Add(order);
                    _context.Baskets.Remove(basket);
                    _context.SaveChangesAsync();
                }
             return response;
            };



            return BadRequest("Problem creating order");
        }
    }
}