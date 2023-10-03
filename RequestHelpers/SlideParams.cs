using ecommerceApi.Entities;

namespace ecommerceApi.RequestHelpers
{
    public class SlideParams : PaginationParams
    {
        public PagesEnum Page { get; set; }
    }
}
