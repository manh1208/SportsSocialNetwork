using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IFollowService
    {
        #region Code from here
        bool FollowUnfollowUser(String userId, String followerId);


        #endregion

        void test();
    }
    public partial class FollowService : IFollowService
    {
        #region Code from here
        public bool FollowUnfollowUser(String userId, String followerId)
        {
            Follow follow = this.FirstOrDefault(x => x.UserId == userId && x.FollowerId == followerId);

            if (follow == null)
            {
                follow = new Follow();
                follow.UserId = userId;
                follow.FollowerId = followerId;
                follow.CreateDate = DateTime.Now;
                follow.Active = true;
                this.Create(follow);
                this.Save();
            }
            else
            {
                if (follow.Active == true)
                {
                    follow.Active = false;

                }
                else {
                    follow.Active = true;
                }

                this.Update(follow);
                this.Save();
            }
            return follow.Active;
        }


        #endregion

        public void test()
        {

        }
    }
}