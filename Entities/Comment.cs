﻿namespace ecommerceApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }

    }
}
