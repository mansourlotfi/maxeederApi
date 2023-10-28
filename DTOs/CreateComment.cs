using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateComment
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Text { get; set; }
        public bool IsActive { get; set; }

    }
}
