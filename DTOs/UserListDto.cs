using ecommerceApi.Entities;

namespace ecommerceApi.DTOs
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<CustomUserRole>? CustomUserRoles { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
