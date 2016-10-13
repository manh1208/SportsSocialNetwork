using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models
{
    public class PostDetailViewModel
    {
        public PostOveralViewModel Post { get; set; }
        public List<PostCommentDetailViewModel> CommentList { get; set; }
    }
}