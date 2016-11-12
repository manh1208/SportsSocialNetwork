using HenchmenWeb.Models.Notifications;
using Microsoft.AspNet.SignalR;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Hubs;
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

            var postCommentService = this.Service<IPostCommentService>();

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
                        //get all relative user of this post
                        List<string> AllRelativeUserIdOfPost = new List<string>();

                        List<PostComment> listPostCmt = postCommentService.GetAllRelativeCmtDistinct(postId).ToList();

                        List<Like> listPostLike = likeService.GetAllRelativeLikeDistinct(postId).ToList();

                        foreach (var item in listPostCmt)
                        {
                            AllRelativeUserIdOfPost.Add(item.UserId);
                        }

                        foreach (var item in listPostLike)
                        {
                            AllRelativeUserIdOfPost.Add(item.UserId);
                        }

                        AllRelativeUserIdOfPost = AllRelativeUserIdOfPost.Distinct().ToList();
                        //end=================


                        Post post = postService.FirstOrDefaultActive(x => x.Id == postId);
                        //noti to post creator
                        if (!(post.UserId.Equals(userId)))
                        {
                            Notification notiForPostCreator = notiService.SaveNoti(post.UserId, userId, "Like", user.FullName + " đã thích bài viết của bạn", (int)NotificationType.Post, post.Id, null, null);

                            //Fire base noti
                            List<string> registrationIds = GetToken(user.Id);

                            //registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

                            NotificationModel model = Mapper.Map<NotificationModel>(PrepareNotificationCustomViewModel(notiForPostCreator));

                            if (registrationIds != null && registrationIds.Count != 0)
                            {
                                Android.Notify(registrationIds, null, model);
                            }


                            //SignalR Noti
                            NotificationFullInfoViewModel notiModelR = notiService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(notiForPostCreator));

                            // Get the context for the Pusher hub
                            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                            // Notify clients in the group
                            hubContext.Clients.User(notiModelR.UserId).send(notiModelR);
                        }


                        //noti to all user that relavtive in this post
                        foreach (var item in AllRelativeUserIdOfPost)
                        {
                            if (!(item.Equals(userId)) && !(item.Equals(post.UserId)))
                            {
                                Notification not = notiService.SaveNoti(item, userId, "Like", user.FullName + " đã thích bài viết mà bạn theo dõi", (int)NotificationType.Post, post.Id, null, null);

                                Notification noti = notiService.FirstOrDefaultActive(n => n.Id == not.Id);

                                //Fire base noti
                                List<string> registrationIds = GetToken(user.Id);

                                //registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

                                NotificationModel model = Mapper.Map<NotificationModel>(PrepareNotificationCustomViewModel(noti));

                                if (registrationIds != null && registrationIds.Count != 0)
                                {
                                    Android.Notify(registrationIds, null, model);
                                }


                                //SignalR Noti
                                NotificationFullInfoViewModel notiModelR = notiService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                                // Get the context for the Pusher hub
                                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                                // Notify clients in the group
                                hubContext.Clients.User(notiModelR.UserId).send(notiModelR);

                            }
                        }

                        post.LatestInteractionTime = DateTime.Now;
                        postService.Update(post);
                        postService.Save();
                        //post.LatestInteractionTime = DateTime.Now;

                        //Notification noti = notiService.SaveNoti(GetPostUserId(postId), userId, "Like", user.FullName + " đã thích bài viết của bạn", (int)NotificationType.Post, postId, null, null);

                        //List<string> registrationIds = GetToken(GetPostUserId(postId));

                        ////registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

                        //if (registrationIds != null && registrationIds.Count != 0)
                        //{
                        //    NotificationModel model = Mapper.Map<NotificationModel>(PrepareNotificationCustomViewModel(noti));

                        //    Android.Notify(registrationIds, null, model);

                        //    //signalR noti
                        //    NotificationFullInfoViewModel notiModelR = notiService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                        //    // Get the context for the Pusher hub
                        //    IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                        //    // Notify clients in the group
                        //    hubContext.Clients.User(notiModelR.UserId).send(notiModelR);
                        //}


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