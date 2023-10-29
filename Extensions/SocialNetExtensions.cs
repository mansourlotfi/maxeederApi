using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class SocialNetExtensions
    {
        public static IQueryable<SocialNetwork> SearchSocialNet(this IQueryable<SocialNetwork> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
