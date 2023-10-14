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

        public static IQueryable<Partner> Filter(this IQueryable<Partner> query,  string? state)
        {
            var stateList = new List<string>();
   
            if (!string.IsNullOrEmpty(state))
                stateList.AddRange(state.ToLower().Split(",").ToList());



            query = query.Where(p => stateList.Count == 0 || stateList.Contains(p.State.ToLower()));
      


            return query;

        }
    }
}
