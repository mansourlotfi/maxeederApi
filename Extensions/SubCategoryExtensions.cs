using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class SubCategoryExtensions
    {
        public static IQueryable<SubCategory> SearchSubCategory(this IQueryable<SubCategory> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
