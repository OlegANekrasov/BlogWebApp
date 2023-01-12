﻿namespace BlogWebApp.BLL.Models
{
    public class AddBlogArticle
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}
