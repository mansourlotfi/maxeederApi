using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateMessageDto
    {
        [Required]
        public string Department { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Tel { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
