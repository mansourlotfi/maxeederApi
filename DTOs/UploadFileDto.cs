using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class UploadFileDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
