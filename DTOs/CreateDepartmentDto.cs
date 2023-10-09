using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.DTOs
{
    public class CreateDepartmentDto
    {
        [Required]
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;


    }
}
