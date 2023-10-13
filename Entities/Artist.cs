namespace ecommerceApi.Entities
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string PictureUrl { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }
        public string? TextEn { get; set; }
    }
}
