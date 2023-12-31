﻿using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateFeatureDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }

    }
}
