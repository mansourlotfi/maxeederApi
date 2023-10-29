using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class QuickAccessExtensions
    {
        public static IQueryable<QuickAccess> SearchQuickAccess(this IQueryable<QuickAccess> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Title.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
