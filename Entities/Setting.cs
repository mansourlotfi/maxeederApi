namespace ecommerceApi.Entities
{
    public class Setting
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? FooterText { get; set; }
        public string? ContactUsRitchText { get; set; }
        public string? ServicesRitchText { get; set; }
        public string? ServicePictureUrl { get; set; }

    }
}
