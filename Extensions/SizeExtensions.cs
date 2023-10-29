
using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class SizeExtensions
    {
        public static IQueryable<Size> SearchSizes(this IQueryable<Size> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.SizeName.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
