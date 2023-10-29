using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class CommentsExtensions
    {

        public static IQueryable<Comment> SearchComment(this IQueryable<Comment> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Text.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
