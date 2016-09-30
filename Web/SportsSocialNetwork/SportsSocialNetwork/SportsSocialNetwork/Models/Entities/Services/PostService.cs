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


        #endregion

        public void test()
        {

        }
    }
}