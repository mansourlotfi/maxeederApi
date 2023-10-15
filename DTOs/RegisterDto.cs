using ecommerceApi.Entities;

namespace ecommerceApi.DTOs
{
    public class RegisterDto : LoginDto
    {
        public string Email { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}