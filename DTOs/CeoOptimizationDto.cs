﻿using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CeoOptimizationDto
    {
        public string? Text { get; set; }
        public string? Description { get; set; }
        public string? MetaTagKeyWords { get; set; }
        public string? MetaTagDescription { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public PageEnum Page { get; set; }
        public bool? IsActive { get; set; } = true;
        
    }
}
