using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPostCommentService {
        IEnumerable<PostComment> GetCommentListByPostId(int postId);

        PostComment Comment(int postId, String userId, String content);
    }

    public partial class PostCommentService
    {
        public IEnumerable<PostComment> GetCommentListByPostId(int postId) {
            return this.GetActive(x => x.PostId == postId);
        }

        public PostComment Comment(int postId, String userId, String content) {
            PostComment comment = new PostComment();
            comment.PostId = postId;
            comment.UserId = userId;
            comment.Comment = content;
            comment.CreateDate = DateTime.Now;
            comment.Active = true;
            this.Create(comment);
            return comment;
        }
    }
}