﻿namespace ecommerceApi.Entities
{
    public class QuickAccess
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int Priority { get; set; }
        public bool? IsActive { get; set; } = true;
        public string? TitleEn { get; set; }


    }
}
