using Microsoft.AspNetCore.Identity;

namespace ecommerceApi.Entities
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }

        public List<CustomUserRole>? CustomUserRoles { get; set; } = new();
        public bool? IsActive { get; set; } = true;
    }
}