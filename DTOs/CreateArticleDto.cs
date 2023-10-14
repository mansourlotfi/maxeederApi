using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateArticleDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public string RitchText { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? TitleEn { get; set; }
        public string? RitchTextEn { get; set; }
        [Required]
        public ArticlesEnum Page { get; set; }
        public string? ShortDesc { get; set; }
        public string? ShortDescEn { get; set; }

    }
}
