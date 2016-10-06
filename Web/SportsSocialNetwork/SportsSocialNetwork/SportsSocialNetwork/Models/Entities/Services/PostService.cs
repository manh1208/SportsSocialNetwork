using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPostService
    {
        #region Code from here
        IEnumerable<Post> GetAll();

        Post GetPostById(int id);

        Post CreatePost(Post model);

        Post EditPost(Post model, bool imageChanged);
        #endregion

        void test();
    }

    public partial class PostService : IPostService
    {
        #region Code from here
        public IEnumerable<Post> GetAll() {
            IEnumerable<Post> postList = this.GetActive();
            return postList;
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
            if (imageChanged)
            {
                post.Image = model.Image;
            }
            this.Update(post);
            return post;
        }


        #endregion

        public void test()
        {

        }
    }
}