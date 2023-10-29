using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class DepartmentExtensions
    {
        public static IQueryable<Department> SearchDepartment(this IQueryable<Department> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
