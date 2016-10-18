using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class PostOveralViewModel
    {
        public int Id { get; set; }
        public String UserId { get; set; }
        public String CreateDate { get; set; }
        public string PostContent { get; set; }
        public String EditDate { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public Nullable<int> GroupId { get; set; }
        public AspNetUserSimpleModel AspNetUser { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool Liked { get; set; }
    }
}