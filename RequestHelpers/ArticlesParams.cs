using ecommerceApi.Entities;

namespace ecommerceApi.RequestHelpers
{

   

    public class ArticlesParams:PaginationParams
    {
        public ArticlesEnum Page { get; set; }

    }
}
