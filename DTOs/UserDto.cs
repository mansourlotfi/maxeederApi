using ecommerceApi.Entities;

namespace ecommerceApi.DTOs
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public BasketDto Basket { get; set; }
        public bool IsActive { get; set; } = true;
    }
}