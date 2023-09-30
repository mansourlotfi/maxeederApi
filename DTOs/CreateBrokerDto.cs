using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateBrokerDto
    {
        [Required]
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Ref { get; set; }
    }
}
