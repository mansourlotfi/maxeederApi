using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class ArticlesExtensions
    {
        public static IQueryable<Article> SearchArticles(this IQueryable<Article> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Title.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
