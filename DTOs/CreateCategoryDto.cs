using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }

        [Required]
        public IFormFile File { get; set; }

        [Required]
        public int Priority { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }

    }
}
