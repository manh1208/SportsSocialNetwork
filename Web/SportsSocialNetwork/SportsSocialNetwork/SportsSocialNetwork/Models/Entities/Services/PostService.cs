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

        Post GetPostById(int id);

        Post CreatePost(Post model);

        Post EditPost(Post model, bool imageChanged);

        AspNetUser GetUserNameOfPost(int postId);

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
            return this.GetActive(x=> x.GroupId==groupId).OrderBy(x => x.EditDate == null ? x.CreateDate : x.EditDate).Skip(skip).Take(take);
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

        public Post EditPost(Post model,bool imageChanged) {
            Post post = FirstOrDefaultActive(x => x.Id == model.Id);
            post.PostContent = model.PostContent;
            post.EditDate = DateTime.Now;
            //if (imageChanged)
            //{
            //    post.Image = model.Image;
            //}
            this.Update(post);
            return post;
        }


        public AspNetUser GetUserNameOfPost(int postId) {
            return this.FirstOrDefaultActive(x => x.Id == postId).AspNetUser;
        }

        #endregion

        public void test()
        {

        }
    }
}