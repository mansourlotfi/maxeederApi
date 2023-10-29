using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class CategoryExtensions
    {
        public static IQueryable<Category> SearchCategory(this IQueryable<Category> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
