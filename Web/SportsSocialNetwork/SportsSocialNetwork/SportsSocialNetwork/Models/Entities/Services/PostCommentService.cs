using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPostCommentService {
        IEnumerable<PostComment> GetCommentListByPostId(int postId);

        IEnumerable<PostComment> GetCommentListByPostId(int postId, int skip, int take);

        PostComment Comment(int postId, String userId, String content, String image);

        string CalculateCommentAge(DateTime date);

        IEnumerable<PostComment> GetAllRelativeCmtDistinct(int postId);
    }

    public partial class PostCommentService
    {
        public IEnumerable<PostComment> GetCommentListByPostId(int postId) {
            return this.GetActive(x => x.PostId == postId);
        }

        public IEnumerable<PostComment> GetCommentListByPostId(int postId, int skip, int take)
        {
            return this.GetActive(x => x.PostId == postId).OrderByDescending(x=> x.CreateDate).Skip(skip).Take(take);
        }

        public PostComment Comment(int postId, String userId, String content, String image) {
            PostComment comment = new PostComment();
            comment.PostId = postId;
            comment.UserId = userId;
            comment.Comment = content;
            comment.Image = image;
            comment.CreateDate = DateTime.Now;
            comment.Active = true;
            this.Create(comment);
            this.Save();
            return comment;
        }

        public string CalculateCommentAge(DateTime date)
        {
            string result = "";
            TimeSpan a = DateTime.Now - date;
            int b = (int)a.TotalSeconds;
            if (b / 86400 > 1)
            {
                result = date.Day.ToString("00") + "/" + date.Month.ToString("00") + "/" + date.Year.ToString("0000")
                    + " lúc " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00");
            }
            else if (b / 3600 < 1)
            {
                int minute = (b % 3600) / 60;
                if (minute == 0)
                {
                    result = "Mới đây";
                }
                else
                {
                    result = minute.ToString() + " phút trước";
                }
            }
            else
            {
                int hours = b / 3600;
                result = hours.ToString() + " giờ trước";
            }
            return result;
        }

        public IEnumerable<PostComment> GetAllRelativeCmtDistinct(int postId)
        {
            return this.GetActive(x => x.PostId == postId).GroupBy(p => p.UserId).Select(p => p.FirstOrDefault());
        }
    }
}