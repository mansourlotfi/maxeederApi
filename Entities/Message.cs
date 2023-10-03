namespace ecommerceApi.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Tel { get; set; }
        public string Text { get; set; }
        public DateTime AddedDate { set; get; }

    }
}
