using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SportsSocialNetwork.Controllers
{
    public class NotificationController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var _notificationService = this.Service<INotificationService>();
            List<Notification> notiList = _notificationService.GetNoti(userId, 0, 20).ToList();
            List<NotificationFullInfoViewModel> notiListVM = new List<NotificationFullInfoViewModel>();

            foreach (var item in notiList)
            {
                notiListVM.Add(_notificationService.PrepareNoti(this.PrepareNotificationViewModel(item)));
            }

            return View();
        }

        [HttpPost]
        public ActionResult LoadNoti(String userId, int skip, int take)
        {
            var service = this.Service<INotificationService>();

            ResponseModel<List<NotificationFullInfoViewModel>> response = null;

            try
            {
                List<Notification> notiList = service.GetNoti(userId, skip, take).ToList();

                List<NotificationFullInfoViewModel> result = new List<NotificationFullInfoViewModel>();

                foreach (var noti in notiList)
                {
                    result.Add(service.PrepareNoti(PrepareNotificationViewModel(noti)));
                }

                response = new ResponseModel<List<NotificationFullInfoViewModel>>(true, "Thông báo mới của bạn:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<NotificationFullInfoViewModel>>.CreateErrorResponse("Không thể tải thông báo", systemError);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult MarkAsRead(int id)
        {
            var service = this.Service<INotificationService>();

            ResponseModel<bool> response = null;

            try
            {
                bool result = service.MarkAsRead(id);
                if (result)
                {
                    response = new ResponseModel<bool>(result, "Đã đánh dấu", null);
                }

                else
                {
                    response = new ResponseModel<bool>(result, "Không có thông báo nào để đánh dấu", null);

                }
            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Không thể đánh dấu", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult MarkAllAsRead(String userId)
        {
            var service = this.Service<INotificationService>();

            ResponseModel<bool> response = null;

            try
            {
                bool result = service.MarkAllAsRead(userId);
                if (result)
                {
                    response = new ResponseModel<bool>(result, "Đã đánh dấu", null);
                }
                else
                {
                    response = new ResponseModel<bool>(result, "Không có thông báo nào để đánh dấu", null);
                }

            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Không thể đánh dấu", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public string GetUnreadCount(string userId)
        {
            var service = this.Service<INotificationService>();

            int result = 0;

            try {
                result = service.GetActive(x => x.UserId.Equals(userId) && (x.MarkRead == false || x.MarkRead == null)).ToList().Count;

            } catch (Exception)
            {
                return "";
            }
            return result.ToString();

        }


        //private NotificationCustomViewModel PrepareNotificationViewModel(Notification noti)
        //{
        //    NotificationCustomViewModel result = Mapper.Map<NotificationCustomViewModel>(noti);

        //    result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

        //    result.Avatar = noti.AspNetUser1.AvatarImage;

        //    return result;

        //}

        private NotificationFullInfoViewModel PrepareNotificationViewModel(Notification noti)
        {
            NotificationFullInfoViewModel result = Mapper.Map<NotificationFullInfoViewModel>(noti);

            result.CreateDateString = result.CreateDate.Value.Day.ToString("00") + "/" + result.CreateDate.Value.Month.ToString("00") + "/" + result.CreateDate.Value.Year.ToString("0000")
                    + " lúc " + result.CreateDate.Value.Hour.ToString("00") + ":" + result.CreateDate.Value.Minute.ToString("00");

            return result;

        }

        public string GetUserId(string id) {
            var service = this.Service<IAspNetUserService>();
            string result = "";
            if(service.FirstOrDefaultActive(x => x.Id.Equals(id)) != null)
            {
                result = service.FirstOrDefaultActive(x => x.Id.Equals(id)).Id;
            }
            return result;
        }

        public AspNetUserViewModel GetUser(string id)
        {
            var service = this.Service<IAspNetUserService>();

            return Mapper.Map<AspNetUserViewModel> (service.FirstOrDefaultActive(x => x.Id.Equals(id)));
        }
    }
}