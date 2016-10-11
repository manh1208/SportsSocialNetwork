﻿using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class LikeController : BaseController
    {
        private String systemError = "An error has occured!";

        [HttpPost]
        public ActionResult LikeUnlikePost(int postId, String userId)
        {
            var service = this.Service<ILikeService>();

            ResponseModel<Like> response = null;
            try
            {
                bool result= service.LikeUnlikePost(postId, userId);

                if (result)
                {
                    response = new ResponseModel<Like>(true, "Liked", null);
                }
                else {
                    response = new ResponseModel<Like>(true, "Unliked", null);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<Like>.CreateErrorResponse("An error has occured!", systemError);
            }
            return Json(response);
        }
    }
}