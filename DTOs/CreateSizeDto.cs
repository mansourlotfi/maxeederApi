using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateSizeDto
    {
        [Required]
        public string SizeName { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? SizeNameEn { get; set; }
    }
}
