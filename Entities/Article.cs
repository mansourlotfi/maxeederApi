namespace ecommerceApi.Entities
{
    public enum ArticlesEnum
    {
        WikiMax = 1,
        MaxPlus,
    
    }
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string PictureUrl { get; set; }
        public int Priority { get; set; }
        public string RitchText { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? TitleEn { get; set; }
        public string? RitchTextEn { get; set; }
        public ArticlesEnum Page { get; set; }
        public DateTime AddedDate { set; get; }


    }
}
