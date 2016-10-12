using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class PostCommentDetailViewModel : PostCommentViewModel
    {
        public AspNetUserOveralViewModel AspNetUser { get; set; }

        public String CreateDateString { get; set; }
    }
}