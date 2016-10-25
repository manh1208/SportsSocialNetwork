using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface INotificationService
    {
        #region Code from here
        Notification SaveNoti(String userId, String title, String message, int type, Nullable<int> postId, Nullable<int> invitationId);

        IEnumerable<Notification> GetNoti(String userId, int skip, int take);
        #endregion

        void test();
    }
    public partial class NotificationService : INotificationService
    {



        #region Code from here
        public Notification SaveNoti(string userId, string title, string message, int type, Nullable<int> postId, Nullable<int> invitationId)
        {
            Notification noti = new Notification();
            noti.UserId = userId;
            noti.Title = title;
            noti.Message = message;
            noti.Type = type;
            noti.PostId = postId;
            noti.InvitationId = invitationId;
            noti.Active = true;
            this.Create(noti);
            this.Save();
            return noti;
        }

        public IEnumerable<Notification> GetNoti(string userId, int skip, int take)
        {
            return this.GetActive(x => x.UserId == userId).OrderBy(x => x.Id).Skip(skip).Take(take);
        }

        #endregion

        public void test()
        {

        }
    }
}