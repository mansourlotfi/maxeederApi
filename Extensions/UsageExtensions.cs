using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class UsageExtensions
    {
        public static IQueryable<Usage> SearchUsage(this IQueryable<Usage> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
