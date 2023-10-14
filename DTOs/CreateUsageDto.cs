using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateUsageDto
    {
        [Required]
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }
    }
}
