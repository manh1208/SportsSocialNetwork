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

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class LikeController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult LikeUnlikePost(int postId, String userId)
        {
            var likeService = this.Service<ILikeService>();

            var notiService = this.Service<INotificationService>();

            var userService = this.Service<IAspNetUserService>();

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
                        Notification noti = notiService.SaveNoti(GetPostUserId(postId),userId,"Like",user.FullName + " đã thích bài viết của bạn", (int)NotificationType.Post,postId,null,null);

                        //List<string> registrationIds = GetToken(user.Id);

                        //NotificationModel model = new NotificationModel();

                        //Temporary
                        List<string> registrationIds = new List<string>();

                        registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

                        NotificationModel model = Mapper.Map< NotificationModel >(PrepareNotificationViewModel(noti));

                        Android.Notify(registrationIds, null, model);
                    }
                }
                else {
                    response = new ResponseModel<Like>(true, "Đã bỏ thích", null);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<Like>.CreateErrorResponse("Thao tác thất bại!", systemError);
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

        private List<string> GetToken(String userId)
        {
            var service = this.Service<IFirebaseTokenService>();

            List<FirebaseToken> tokenList = service.Get(x => x.UserId.Equals(userId)).ToList();

            List<string> registrationIds = new List<string>();

            foreach(var token in tokenList)
            {
                registrationIds.Add(token.Token);
            }

            return registrationIds;

        }

        private String GetPostUserId(int postId) {
            var service = this.Service<IPostService>();

            return service.FirstOrDefaultActive(x => x.Id == postId).UserId;
        }

    }
}