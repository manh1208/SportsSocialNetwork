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

namespace SportsSocialNetwork.Controllers
{
    public class NotificationController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult LoadNoti(String userId, int skip, int take)
        {
            var service = this.Service<INotificationService>();

            ResponseModel<List<NotificationCustomViewModel>> response = null;

            try
            {
                List<Notification> notiList = service.GetNoti(userId, skip, take).ToList();

                List<NotificationCustomViewModel> result = new List<NotificationCustomViewModel>();

                foreach (var noti in notiList)
                {
                    result.Add(PrepareNotificationViewModel(noti));
                }

                response = new ResponseModel<List<NotificationCustomViewModel>>(true, "Thông báo mới của bạn:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<NotificationCustomViewModel>>.CreateErrorResponse("Không thể tải thông báo", systemError);
            }
            return Json(response);
        }

        private NotificationCustomViewModel PrepareNotificationViewModel(Notification noti)
        {
            NotificationCustomViewModel result = Mapper.Map<NotificationCustomViewModel>(noti);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.Avatar = noti.AspNetUser1.AvatarImage;

            return result;

        }

        public string GetUserId(string id) {
            var service = this.Service<IAspNetUserService>();

            string result = service.FirstOrDefaultActive(x => x.Id.Equals(id)).Id;

            return result;
        }
    }
}