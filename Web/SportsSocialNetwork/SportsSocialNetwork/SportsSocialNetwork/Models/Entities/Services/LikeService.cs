using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{

    public partial interface ILikeService
    {
        #region Code from here
        IEnumerable<Like> GetLikeListByPostId(int postId);

        Like LikePost(int postId, String userId);

        #endregion

        void test();
    }
    public partial class LikeService : ILikeService
    {
        #region Code from here
        public IEnumerable<Like> GetLikeListByPostId(int postId)
        {
            return this.GetActive(x => x.PostId == postId);
        }

        public Like LikePost(int postId, String userId) {
            Like like = new Like();
            like.UserId = userId;
            like.PostId = postId;
            like.CreateDate = DateTime.Now;
            like.Active = true;
            this.Create(like);
            this.Save();
            return like;
        }

        #endregion

        public void test()
        {

        }
    }
}