using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPostService
    {
        #region Code from here
        IEnumerable<Post> GetAll(int skip, int take);

        IEnumerable<Post> GetAllPostsOfGroup(int groupId, int skip, int take);

        IEnumerable<Post> GetAllPostOfUser(string userId, int skip, int take);

        Post GetPostById(int id);

        Post CreatePost(Post model);

        Post EditPost(Post model);

        AspNetUser GetUserNameOfPost(int postId);

        string CalculatePostAge(DateTime date);

        #endregion

        void test();
    }

    public partial class PostService : IPostService
    {
        #region Code from here
        public IEnumerable<Post> GetAll(int skip, int take) {
            IEnumerable<Post> postList = this.GetActive().OrderBy(x=> x.EditDate==null? x.CreateDate: x.EditDate).Skip(skip).Take(take);
            return postList;
        }

        public IEnumerable<Post> GetAllPostsOfGroup(int groupId, int skip, int take) {
            return this.GetActive(x=> x.GroupId==groupId).OrderByDescending(x => x.EditDate == null ? x.CreateDate : x.EditDate).Skip(skip).Take(take);
        }

        public IEnumerable<Post> GetAllPostOfUser(string userId, int skip, int take)
        {
            return this.GetActive(x => x.UserId == userId).OrderByDescending(x => x.EditDate == null ? x.CreateDate : x.EditDate).Skip(skip).Take(take);
        }

        public Post GetPostById(int id) {
            return this.FirstOrDefaultActive(x => x.Id == id);
        }

        public Post CreatePost(Post model) {
            model.CreateDate = DateTime.Now;
            model.Active = true;
            this.Create(model);
            return model;
        }

        public Post EditPost(Post model) {
            Post post = FirstOrDefaultActive(x => x.Id == model.Id);
            post.PostContent = model.PostContent;
            post.EditDate = DateTime.Now;
            this.Update(post);
            return post;
        }


        public AspNetUser GetUserNameOfPost(int postId) {
            return this.FirstOrDefaultActive(x => x.Id == postId).AspNetUser;
        }

        public string CalculatePostAge(DateTime date)
        {
            string result = "";
            TimeSpan a = DateTime.Now - date;
            int b = (int)a.TotalSeconds;
            if ((b / 86400) >= 1)
            {
                result = date.Day + "/" + date.Month + "/" + date.Year + " lúc " + date.Hour + ":" + date.Minute;
            }
            else if ((b / 3600) < 1)
            {
                int minute = (b % 3600)/60;
                if (minute == 0)
                {
                    result = "Mới tức thì";
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

        #endregion

        public void test()
        {

        }
    }
}