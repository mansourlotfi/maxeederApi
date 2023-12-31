﻿using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        //[Required]
        //[Range(100, Double.PositiveInfinity)]
        public long? Price { get; set; }

        public IFormFile? File { get; set; }

        public int? SubCategoryId { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        [Range(0, 200)]
        public int QuantityInStock { get; set; }
        public bool? IsFeatured { get; set; } = false;
        public List<int>? Features { get; set; } = new List<int>();
        public bool? IsActive { get; set; } = true;
        [Required]
        public string? Usage { get; set; }
        public string? Size { get; set; }
        public string? NameEn { get; set; }
        public string? DescriptionEn { get; set; }
        [Required]
        public int Priority { get; set; }
        public bool? ShowPrice { get; set; } = true;

    }
}
