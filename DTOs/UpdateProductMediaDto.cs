using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class UpdateProductMediaDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
