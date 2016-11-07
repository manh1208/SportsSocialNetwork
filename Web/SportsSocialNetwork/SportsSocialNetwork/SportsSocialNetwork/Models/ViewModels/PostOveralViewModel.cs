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
        public DateTime CreateDate { get; set; }
        public String CreateDateString { get; set; }
        public string PostContent { get; set; }
        public int ContentType { get; set; }
        public DateTime EditDate { get; set; }
        public String EditDateString { get; set; }
        public List<PostImageViewModel> PostImages { get; set; }
        public bool Active { get; set; }
        public string ProfileId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public AspNetUserSimpleModel AspNetUser { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool Liked { get; set; }
    }
}