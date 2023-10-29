using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class SlideExtensions
    {
        public static IQueryable<Slide> SearchSlide(this IQueryable<Slide> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
