using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class ArtistExtensions
    {
        public static IQueryable<Artist> SearchArtist(this IQueryable<Artist> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
