using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        public IFormFile? File { get; set; }
        [Required]
        public int Priority { get; set; }
    }
}
