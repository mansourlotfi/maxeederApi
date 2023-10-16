namespace ecommerceApi.Entities
{

    public enum PageItemsEnum
    {
        ServiceMain = 1,
        ServiceItems,
        ContactUs,
        AboutUsHeader,
        AboutUsHistory,
        AboutUsTestimonials,
        AboutUsWarranty,
        AboutUsHonors,
        CoWorkersHead,
        CoWorkersTabs,
        CoWorkersWarranty,
    }

    public class PageItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Link { get; set; }
        public string? PictureUrl { get; set; }
        public int Priority { get; set; }
        public string? RitchText { get; set; }
        public PageItemsEnum Page { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? TitleEn { get; set; }
        public string? TextEn { get; set; }
        public string? ShortDesc { get; set; }
        public string? ShortDescEn { get; set; }

    }
}
