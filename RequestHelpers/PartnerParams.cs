namespace ecommerceApi.RequestHelpers
{
    public class PartnerParams:PaginationParams
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}
