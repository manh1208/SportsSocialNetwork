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
        private String systemError = "An error has occured!";

        [HttpPost]
        public ActionResult FollowUnfollowUser(String userId, String followerId) {
            var service = this.Service<IFollowService>();

            ResponseModel<bool> response = null;

            try {
                bool result = service.FollowUnfollowUser( userId, followerId);

                if (result)
                {
                    response = new ResponseModel<bool>(true, "Followed", null);
                }
                else {
                    response = new ResponseModel<bool>(true, "Unfollowed", null);
                }

            } catch (Exception) {
                response = ResponseModel<bool>.CreateErrorResponse("Follow failed",systemError);
            }

            return Json(response);
        }
    }
}