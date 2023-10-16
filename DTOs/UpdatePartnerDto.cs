using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class UpdatePartnerDto
    {
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? CEO { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Tel { get; set; }
        public string? Long { get; set; }
        public string? Lat { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? TitleEn { get; set; }
        public string? CeoEn { get; set; }
        public string? StateEn { get; set; }
        public string? CityEn { get; set; }
        public string? Address { get; set; }
        public string? AddressEn { get; set; }

    }
}
