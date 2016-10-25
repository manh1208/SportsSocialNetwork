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

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class NotificationController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult LoadNoti(String userId, int skip, int take) {
            var service = this.Service<INotificationService>();

            ResponseModel<List<NotificationViewModel>> response = null;

            try {
                List<Notification> notiList = service.GetNoti(userId, skip, take).ToList();

                List<NotificationViewModel> result = Mapper.Map<List<NotificationViewModel>>(notiList);

                response = new ResponseModel<List<NotificationViewModel>>(true, "Thông báo mới của bạn:", null, result);
            } catch (Exception) {
                response = ResponseModel<List<NotificationViewModel>>.CreateErrorResponse("Không thể tải thông báo", systemError);
            }
            return Json(response);
        }
    }
}