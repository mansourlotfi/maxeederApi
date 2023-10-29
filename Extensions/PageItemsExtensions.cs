using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class PageItemsExtensions
    {
        public static IQueryable<PageItem> SearchPageItem(this IQueryable<PageItem> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Title.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
