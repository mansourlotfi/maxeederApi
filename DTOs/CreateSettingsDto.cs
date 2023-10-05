using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateSettingsDto
    {
        public string? Description { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? FooterText { get; set; }
        public string? ContactUsRitchText { get; set; }
        public string? ServicesRitchText { get; set; }
        public IFormFile? File { get; set; }
    }
}
