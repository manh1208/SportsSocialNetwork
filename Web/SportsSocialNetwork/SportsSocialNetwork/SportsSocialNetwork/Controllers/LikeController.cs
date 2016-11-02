using HenchmenWeb.Models.Notifications;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Notifications;
using SportsSocialNetwork.Models.ViewModels;
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

        [HttpPost]
        public ActionResult LikeUnlikePost(int postId, String userId)
        {
            var likeService = this.Service<ILikeService>();

            var notiService = this.Service<INotificationService>();

            var userService = this.Service<IAspNetUserService>();

            var postService = this.Service<IPostService>();

            ResponseModel<Like> response = null;
            try
            {
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
                        Post post = postService.FirstOrDefaultActive(x => x.Id == postId);

                        post.LatestInteractionTime = DateTime.Now;

                        Notification noti = notiService.SaveNoti(GetPostUserId(postId), userId, "Like", user.FullName + " đã thích bài viết của bạn", (int)NotificationType.Post, postId, null, null);

                        List<string> registrationIds = GetToken(GetPostUserId(postId));

                        //registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

                        if (registrationIds != null && registrationIds.Count != 0)
                        {
                            NotificationModel model = Mapper.Map<NotificationModel>(PrepareNotificationCustomViewModel(noti));

                            Android.Notify(registrationIds, null, model);
                        }


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

        private NotificationCustomViewModel PrepareNotificationCustomViewModel(Notification noti)
        {
            NotificationCustomViewModel result = Mapper.Map<NotificationCustomViewModel>(noti);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.Avatar = noti.AspNetUser1.AvatarImage;

            return result;

        }

        private List<string> GetToken(String userId)
        {
            var service = this.Service<IFirebaseTokenService>();

            List<FirebaseToken> tokenList = service.Get(x => x.UserId.Equals(userId)).ToList();

            List<string> registrationIds = new List<string>();
            if (tokenList != null)
            {
                foreach (var token in tokenList)
                {
                    registrationIds.Add(token.Token);
                }
            }

            return registrationIds;
        }
    }
}