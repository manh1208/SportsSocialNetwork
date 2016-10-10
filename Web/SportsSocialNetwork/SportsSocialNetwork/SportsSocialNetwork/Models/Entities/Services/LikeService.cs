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

        bool LikeUnlikePost(int postId, String userId);

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

        public bool LikeUnlikePost(int postId, String userId) {
            Like like = FirstOrDefault(x=> x.PostId==postId&& x.UserId==userId);
            if (like == null)
            {
                like = new Like();
                like.UserId = userId;
                like.PostId = postId;
                like.CreateDate = DateTime.Now;
                like.Active = true;
                this.Create(like);
                this.Save();
            }

            else {
                if (like.Active == true)
                {
                    like.Active = false;
                }
                else {
                    like.Active = true;
                }
                this.Update(like);
                this.Save();
            }
            return like.Active;
        }

        #endregion

        public void test()
        {

        }
    }
}