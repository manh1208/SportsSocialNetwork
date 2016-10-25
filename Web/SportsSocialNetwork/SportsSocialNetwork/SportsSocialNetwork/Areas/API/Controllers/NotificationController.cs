﻿using SkyWeb.DatVM.Mvc;
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

        [HttpPost]
        public ActionResult MarkAsRead(int id) {
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
                if (result) {
                    response = new ResponseModel<bool>(result, "Đã đánh dấu", null);
                }
                else {
                    response = new ResponseModel<bool>(result, "Không có thông báo nào để đánh dấu", null);
                }

            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Không thể đánh dấu", systemError);
            }

            return Json(response);
        }
    }
}