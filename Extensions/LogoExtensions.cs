using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class LogoExtensions
    {
        public static IQueryable<Logo> SearchLogo(this IQueryable<Logo> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
