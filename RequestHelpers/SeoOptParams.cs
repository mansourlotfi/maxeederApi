using ecommerceApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace ecommerceApi.RequestHelpers
{
    public class SeoOptParams:PaginationParams
    {
        [Required]
        public PageEnum Page { get; set; }

    }
}
