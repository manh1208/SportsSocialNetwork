using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class PostOveralViewModel : PostViewModel
    {
        public String CreateDateString { get; set; }
        public String EditDateString { get; set; }
        public String UserName { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool Liked { get; set; }

        public void CreateDateStrings()
        {
            this.CreateDateString = this.CreateDate.ToString();
            this.EditDateString = this.EditDate.ToString();
        }
    }
}