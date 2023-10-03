using ecommerceApi.Entities;

namespace ecommerceApi.RequestHelpers
{
    public class PageItemParams:PaginationParams
    {
        public PageItemsEnum Page { get; set; }

    }
}
