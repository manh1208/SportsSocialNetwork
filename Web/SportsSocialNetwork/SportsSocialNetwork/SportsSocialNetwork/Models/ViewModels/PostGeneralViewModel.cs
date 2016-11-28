using SportsSocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class PostGeneralViewModel : PostViewModel
    {
        public List<PostImageViewModel> PostImages { get; set; }
        public AspNetUserSimpleModel AspNetUser { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool Liked { get; set; }
        public string PostAge { get; set; }
        public List<PostCommentDetailViewModel> PostComments { get; set; }
        public List<PostSportDetailViewModel> PostSports { get; set; }
        public float PostRank { get; set; }
        public float PostWeight { get; set; }
        public float TimeDecay { get; set; }
        public AspNetUserSimpleModel Profile { get; set; }

        //====================================
        public EventShareViewModel Event { get; set; }
        public OrderShareViewModel Order { get; set; }
        public PostOveralViewModel Post2 { get; set; }
        public NewsViewModel News { get; set; }
    }
}