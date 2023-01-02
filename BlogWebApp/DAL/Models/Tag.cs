﻿namespace BlogWebApp.DAL.Models
{
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<BlogArticle> BlogArticles { get; set; }
    }
}
