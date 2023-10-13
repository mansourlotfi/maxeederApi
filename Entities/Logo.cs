namespace ecommerceApi.Entities
{
    public class Logo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string PictureUrl { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? NameEn { get; set; }

    }
}
