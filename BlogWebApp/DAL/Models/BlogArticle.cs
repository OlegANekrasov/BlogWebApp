﻿using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlogWebApp.DAL.Models
{
    public class BlogArticle
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateChange { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }

        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
