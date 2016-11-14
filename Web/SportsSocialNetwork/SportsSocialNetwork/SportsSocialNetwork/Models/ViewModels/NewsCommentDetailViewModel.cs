using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class NewsCommentDetailViewModel : NewsCommentViewModel
    {
        public AspNetUserViewModel AspNetUser { get; set; }
        public string CommentAge { get; set; }
    }
}