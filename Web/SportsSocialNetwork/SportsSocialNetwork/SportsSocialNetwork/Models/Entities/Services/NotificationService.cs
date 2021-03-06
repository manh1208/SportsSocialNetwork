﻿
using SportsSocialNetwork.Controllers;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface INotificationService
    {
        #region Code from here
        Notification SaveNoti(String userId, String fromUserId, String title, String message, int type, Nullable<int> postId, Nullable<int> invitationId, Nullable<int> orderId);

        Notification CreateNoti(string userId, string fromUserId, string title, string message, int type, Nullable<int> postId, Nullable<int> invitationId, Nullable<int> orderId, Nullable<int> groupId);

        IEnumerable<Notification> GetNoti(String userId, int skip, int take);

        bool MarkAsRead(int id);

        bool MarkAllAsRead(String userId);

        NotificationFullInfoViewModel PrepareNoti(NotificationFullInfoViewModel model);
        #endregion

        void test();
    }
    public partial class NotificationService : INotificationService
    {



        #region Code from here
        public Notification SaveNoti(string userId, String fromUserId, string title, string message, int type, Nullable<int> postId, Nullable<int> invitationId, Nullable<int> orderId)
        {
            Notification noti = new Notification();
            noti.UserId = userId;
            noti.FromUserId = fromUserId;
            noti.Title = title;
            noti.Message = message;
            noti.Type = type;
            noti.PostId = postId;
            noti.CreateDate = DateTime.Now;
            noti.InvitationId = invitationId;
            noti.OrderId = orderId;
            noti.MarkRead = false;
            noti.Active = true;
            this.Create(noti);
            this.Save();
            return noti;
        }

        public Notification CreateNoti(string userId, string fromUserId, string title, string message, int type, Nullable<int> postId, Nullable<int> invitationId, Nullable<int> orderId, Nullable<int> groupId)
        {
            Notification noti = new Notification();
            noti.UserId = userId;
            noti.FromUserId = fromUserId;
            noti.Title = title;
            noti.Message = message;
            noti.Type = type;
            noti.PostId = postId;
            noti.CreateDate = DateTime.Now;
            noti.InvitationId = invitationId;
            noti.OrderId = orderId;
            noti.GroupId = groupId;
            noti.MarkRead = false;
            noti.Active = true;
            this.Create(noti);
            this.Save();
            return noti;
        }

        public IEnumerable<Notification> GetNoti(string userId, int skip, int take)
        {
            return this.GetActive(x => x.UserId == userId).OrderByDescending(x => x.Id).Skip(skip).Take(take);
        }

        public bool MarkAsRead(int id)
        {
            Notification noti = FirstOrDefaultActive(x => x.Id == id);
            if (noti == null) {
                return false;
            }
            noti.MarkRead = true;
            this.Save();
            return true;
        }

        public bool MarkAllAsRead(string userId)
        {
            List<Notification> notiList = GetActive(x => x.UserId == userId).ToList();
            if (notiList == null||notiList.Count==0)
            {
                return false;
            }
            foreach (var noti in notiList) {
                noti.MarkRead = true;
            }
            this.Save();
            return true;
        }

        public NotificationFullInfoViewModel PrepareNoti(NotificationFullInfoViewModel model)
        {
            NotificationController controller = new NotificationController();

            model.AspNetUser = controller.GetUser(model.UserId);

            model.AspNetUser1 = controller.GetUser(model.FromUserId);
            if (model.CreateDate.HasValue)
            {

                model.CreateDateString = model.CreateDate.Value.Day.ToString("00") + "/" + model.CreateDate.Value.Month.ToString("00") + "/" + model.CreateDate.Value.Year.ToString("0000")
                        + " lúc " + model.CreateDate.Value.Hour.ToString("00") + ":" + model.CreateDate.Value.Minute.ToString("00");

            }else
            {
                model.CreateDateString = "11/11/2016 lúc 00:00";
            }
            return model;
        }

        #endregion

        public void test()
        {

        }


    }
}