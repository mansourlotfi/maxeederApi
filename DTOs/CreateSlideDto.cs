using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateSlideDto
    {
        [Required]
        public string Name { get; set; }
        public string Link { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public PagesEnum Page { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }

    }
}
