﻿namespace BlogWebApp.BLL.Models
{
    public class EditComment
    {
        public string Id { get; set; }

        public string BlogArticleId { get; set; }

        public string Content { get; set; }

        public byte[]? Image { get; set; }
    }
}
