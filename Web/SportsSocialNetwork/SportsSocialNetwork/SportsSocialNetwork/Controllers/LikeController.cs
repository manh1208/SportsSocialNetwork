﻿using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class LikeController : BaseController
    {
        private string[] systemError;

        // GET: Like
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LikeUnlikePost(int postId, String userId)
        {
            var likeService = this.Service<ILikeService>();

            var notiService = this.Service<INotificationService>();

            var userService = this.Service<IAspNetUserService>();

            var postService = this.Service<IPostService>();

            ResponseModel<Like> response = null;
            try
            {
                Post post = postService.FirstOrDefaultActive(p => p.Id == postId);
                
                AspNetUser user = userService.FirstOrDefaultActive(x => x.Id == userId);

                bool createNoti = false;

                Like like = likeService.FirstOrDefault(x => x.PostId == postId && x.UserId == userId);

                if (like == null && GetPostUserId(postId) != user.Id)
                {
                    createNoti = true;
                }

                like = likeService.LikeUnlikePost(postId, userId);

                bool result = like.Active;

                if (result)
                {
                    response = new ResponseModel<Like>(true, "Đã thích", null);
                    if (createNoti)
                    {
                        notiService.SaveNoti(GetPostUserId(postId), userId, "Like", user.FullName + " đã thích bài viết của bạn", (int)NotificationType.Post, postId, null, null);
                        post.LatestInteractionTime = DateTime.Now;
                        postService.Update(post);
                        postService.Save();
                    }
                }
                else
                {
                    response = new ResponseModel<Like>(true, "Đã bỏ thích", null);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<Like>.CreateErrorResponse("Thao tác thất bại!", systemError);
            }
            return Json(response);
        }

        private String GetPostUserId(int postId)
        {
            var service = this.Service<IPostService>();

            return service.FirstOrDefaultActive(x => x.Id == postId).UserId;
        }
    }
}