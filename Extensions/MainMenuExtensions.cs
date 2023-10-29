using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class MainMenuExtensions
    {
        public static IQueryable<MainMenu> SearchMainMenu(this IQueryable<MainMenu> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Title.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
