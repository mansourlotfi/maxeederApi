using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateBrandDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public bool? IsActive { get; set; } = true;

    }
}
