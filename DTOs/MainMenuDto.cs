using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class MainMenuDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public int Priority { get; set; }
    }
}
