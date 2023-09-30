using ecommerceApi.Data;
using ecommerceApi.DTOs;
using ecommerceApi.Entities.OrderAggregate;
using ecommerceApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerceApi.Controllers

{
    public class PaymentsController : BaseApiController
    {
        private readonly StoreContext _context;
        public PaymentsController( StoreContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent()
        {
            var basket = await _context.Baskets
                .RetrieveBasketWithItems(User.Identity.Name)
                .FirstOrDefaultAsync();

            if (basket == null) return NotFound();


            basket.Order_id = basket.Order_id ?? Guid.NewGuid().ToString("N");

            _context.Update(basket);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest(new ProblemDetails { Title = "Problem updating basket with intent" });

            return basket.MapBasketToDto();
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
           
            //var charge ;

            //var order = await _context.Orders.FirstOrDefaultAsync(x =>
            //    x.Order_id == charge.Order_id);

            //if (charge.Status == "succeeded") order.OrderStatus = OrderStatus.PaymentReceived;

            await _context.SaveChangesAsync();

            return new EmptyResult();
        }
    }
}