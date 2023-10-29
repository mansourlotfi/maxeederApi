using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class BrandsExtensions
    {
        public static IQueryable<Brand> SearchBrands(this IQueryable<Brand> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
