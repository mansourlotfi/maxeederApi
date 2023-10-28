using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateProductCommentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
