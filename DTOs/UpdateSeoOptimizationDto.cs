using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class UpdateSeoOptimizationDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? TextEn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionEn { get; set; }
        public string? MetaTagKeyWords { get; set; }
        public string? MetaTagKeyWordsEn { get; set; }
        public string? MetaTagDescription { get; set; }
        public string? MetaTagDescriptionEn { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public PageEnum Page { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
