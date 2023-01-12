﻿using BlogWebApp.DAL.Models;
using System.Collections.Generic;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    public class BlogArticleViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string DateCreation { get; set; }

        public string UserId { get; set; }

        public string Author { get; set; }

        public string Tags { get; set; }

        public bool IsEdit { get; set; } = false;

        public BlogArticleViewModel(BlogArticle blogArticle, User user)
        {
            Id = blogArticle.Id;
            Title = blogArticle.Title;
            DateCreation = blogArticle.DateCreation.ToString("dd.MM.yyyy");
            UserId = blogArticle.UserId;
            Author = blogArticle.User.Email;

            if(user == blogArticle.User) IsEdit = true;

            bool first = true;
            var tags = blogArticle.Tags;
            foreach (var item in tags.OrderBy(o => o.Name))
            {
                if(first)
                {
                    Tags = item.Name;
                    first = false; 
                }
                else
                {
                    Tags += ", " + item.Name;
                }
            }
        }
    }
}
