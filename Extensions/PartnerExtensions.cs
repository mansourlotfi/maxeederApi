using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public static class PartnerExtensions
    {
        public static IQueryable<Partner> Search(this IQueryable<Partner> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Title.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Partner> Filter(this IQueryable<Partner> query,  string? city)
        {
            var cityList = new List<string>();
   


            if (!string.IsNullOrEmpty(city))
                cityList.AddRange(city.ToLower().Split(",").ToList());



            query = query.Where(p => cityList.Count == 0 || cityList.Contains(p.City.ToLower()));
      


            return query;

        }
    }
}
