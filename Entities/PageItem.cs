namespace ecommerceApi.Entities
{

    public enum PageItemsEnum
    {
        ServiceMain = 1,
        ServiceItems,
        MaxPlus,
        WikiMax,
        ContactUs,
        AboutUsHeader,
        AboutUsHistory,
        AboutUsTestimonials,
        AboutUsWarranty,
        AboutUsHonors
    }

    public class PageItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Link { get; set; }
        public string? PictureUrl { get; set; }
        public int Priority { get; set; }
        public PageItemsEnum Page { get; set; }

    }
}
