using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class PostCommentDetailViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public String CreateDateString { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public AspNetUserSimpleModel AspNetUser { get; set; }
    }
}