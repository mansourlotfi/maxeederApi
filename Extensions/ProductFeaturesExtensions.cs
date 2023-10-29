using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class ProductFeaturesExtensions
    {
        public static IQueryable<Feature> SearchProductFeatures(this IQueryable<Feature> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
