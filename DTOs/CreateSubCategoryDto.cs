﻿using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateSubCategoryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }

        [Required]
        public IFormFile File { get; set; }

        [Required]
        public int Priority { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
