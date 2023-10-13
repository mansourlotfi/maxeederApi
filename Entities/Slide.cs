namespace ecommerceApi.Entities
{
    public enum PagesEnum
    {
        Home = 1,
        Products,
        Services,
        MaxPlus,
        WikiMax,
        ContactUs,
        AboutUs,
        Coworkers
    }

    public class Slide
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string PictureUrl { get; set; }
        public int Priority { get; set; }
        public PagesEnum Page { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }


    }
}
