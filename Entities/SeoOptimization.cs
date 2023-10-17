namespace ecommerceApi.Entities
{

    public enum PageEnum
    {
        home = 1,
        Products,
        Services,
        MaxPlus,
        WikiMax,
        ContactUs,
        AboutUs,
        Partners
    }

    public class SeoOptimization
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
        public string? MetaTagKeyWords { get; set; }
        public string? MetaTagDescription { get; set; }
        public int Priority { get; set; }
        public PageEnum Page { get; set; }
        public bool? IsActive { get; set; } = true;


    }
}
