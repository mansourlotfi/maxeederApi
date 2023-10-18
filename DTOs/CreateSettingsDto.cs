using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateSettingsDto
    {
        [Required]
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? AddressEn { get; set; }
        public string? Phone { get; set; }
        public string? FooterText { get; set; }
        public string? FooterTextEn { get; set; }
        public string? WorkHours { get; set; }
        public string? WorkHoursEn { get; set; }

    }
}
