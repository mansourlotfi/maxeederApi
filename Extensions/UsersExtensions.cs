using ecommerceApi.DTOs;


namespace ecommerceApi.Extensions
{
    public static class UsersExtensions
    {
        public static IQueryable<UserListDto> SearchUser(this IQueryable<UserListDto> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.PhoneNumber.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
