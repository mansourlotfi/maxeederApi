﻿using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class UpdatePageItemDto
    {
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Link { get; set; }
        public IFormFile? File { get; set; }
        [Required]
        public int Priority { get; set; }
        public string? RitchText { get; set; }

        [Required]
        public PageItemsEnum Page { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? TitleEn { get; set; }
        public string? TextEn { get; set; }
        public string? ShortDesc { get; set; }
        public string? ShortDescEn { get; set; }
    }
}
