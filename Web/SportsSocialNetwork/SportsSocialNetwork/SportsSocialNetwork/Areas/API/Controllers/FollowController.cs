using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class FollowController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult FollowUnfollowUser(String userId, String followerId) {
            var service = this.Service<IFollowService>();

            ResponseModel<bool> response = null;

            try {
                bool result = service.FollowUnfollowUser( userId, followerId);

                if (result)
                {
                    response = new ResponseModel<bool>(true, "Đã theo dõi", null);
                }
                else {
                    response = new ResponseModel<bool>(true, "Đã bỏ theo dõi", null);
                }

            } catch (Exception) {
                response = ResponseModel<bool>.CreateErrorResponse("Theo dõi thất bại",systemError);
            }

            return Json(response);
        }
    }
}