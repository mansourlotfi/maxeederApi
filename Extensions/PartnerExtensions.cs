using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class PartnerExtensions
    {
        public static IQueryable<Partner> Search(this IQueryable<Partner> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Title.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
