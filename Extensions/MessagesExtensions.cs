using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class MessagesExtensions
    {

        public static IQueryable<Message> SearchMessage(this IQueryable<Message> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Text.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
