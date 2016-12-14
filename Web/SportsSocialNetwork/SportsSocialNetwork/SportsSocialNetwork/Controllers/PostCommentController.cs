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
using Teek.Models;

namespace SportsSocialNetwork.Controllers
{
    public class PostCommentController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        private String userImagePath = "PostComment";

        [HttpPost]
        public ActionResult GetComment(int postId, int skip, int take)
        {
            var service = this.Service<IPostCommentService>();

            ResponseModel<List<PostCommentDetailViewModel>> response = null;

            try
            {
                List<PostComment> commentList = service.GetCommentListByPostId(postId, skip, take).ToList();

                List<PostCommentDetailViewModel> result = new List<PostCommentDetailViewModel>();

                foreach (var comment in commentList)
                {
                    result.Add(PreparePostCommentDetailViewModel(comment));
                }

                response = new ResponseModel<List<PostCommentDetailViewModel>>(true, "Tải bình luận thành công", null, result);


            }
            catch (Exception)
            {
                response = ResponseModel<List<PostCommentDetailViewModel>>.CreateErrorResponse("Tải bình luận thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Comment(int postId, String userId, String content, HttpPostedFileBase image)
        {
            var service = this.Service<IPostCommentService>();

            var notiService = this.Service<INotificationService>();

            var postService = this.Service<IPostService>();

            var aspNetUserService = this.Service<IAspNetUserService>();

            var likeService = this.Service<ILikeService>();

            ResponseModel<PostCommentDetailViewModel> response = null;

            try
            {
                String commentImage = null;

                if (image != null)
                {
                    FileUploader uploader = new FileUploader();

                    commentImage = uploader.UploadImage(image, userImagePath);
                }

                PostComment comment = service.Comment(postId, userId, content, commentImage);

                AspNetUser commentedUser = aspNetUserService.FindUser(comment.UserId);

                Post post = postService.GetPostById(comment.PostId);

                AspNetUser user = postService.GetUserNameOfPost(post.Id);

                List<string> AllRelativeUserIdOfPost = new List<string>();

                List<PostComment> listPostCmt = service.GetAllRelativeCmtDistinct(postId).ToList();

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

                //Noti to post creator
                if (!(post.UserId.Equals(commentedUser.Id)))
                {
                    string u = post.UserId;
                    string u1 = commentedUser.Id;
                    Notification notiForPostCreator = notiService.SaveNoti(u, u1, "Comment", commentedUser.FullName + " đã bình luận về bài viết của bạn", int.Parse(NotificationType.Post.ToString("d")), post.Id, null, null);

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
                    if (!(item.Equals(commentedUser.Id)) && !(item.Equals(post.UserId)))
                    {
                        string i = item;
                        string i1 = commentedUser.Id;
                        Notification not = notiService.SaveNoti(item, commentedUser.Id, "Comment", commentedUser.FullName + " đã bình luận về bài viết mà bạn theo dõi", int.Parse(NotificationType.Post.ToString("d")), post.Id, null, null);

                        Notification noti = notiService.FirstOrDefaultActive(n => n.Id == not.Id);

                        //Fire base noti
                        List <string> registrationIds = GetToken(item);

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

                

                PostCommentDetailViewModel result = PreparePostCommentDetailViewModel(comment);

                response = new ResponseModel<PostCommentDetailViewModel>(true, "Bình luận thành công", null, result);
                post.LatestInteractionTime = DateTime.Now;
                postService.Update(post);
                postService.Save();

            }
            catch (Exception)
            {
                response = ResponseModel<PostCommentDetailViewModel>.CreateErrorResponse("Bình luận thất bại", systemError);
            }
            return Json(response);
        }

        public ActionResult LoadSavedComment(String cmtId)
        {
            int id = Int32.Parse(cmtId);
            var result = new AjaxOperationResult<PostCommentDetailViewModel>();
            var _postCommentService = this.Service<IPostCommentService>();
            PostComment tmp = _postCommentService.FirstOrDefaultActive(p => p.Id == id);
            if (tmp != null)
            {
                var _userService = this.Service<IAspNetUserService>();
                PostCommentDetailViewModel cmtGeneral = Mapper.Map<PostCommentDetailViewModel>(tmp);
                cmtGeneral.CommentAge = _postCommentService.CalculateCommentAge(cmtGeneral.CreateDate);
                result.AdditionalData = cmtGeneral;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        private PostCommentDetailViewModel PreparePostCommentDetailViewModel(PostComment comment)
        {
            PostCommentDetailViewModel result = Mapper.Map<PostCommentDetailViewModel>(comment);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            return result;
        }

        public ActionResult DeleteComment(int id)
        {
            var result = new AjaxOperationResult();
            var _service = this.Service<IPostCommentService>();

            PostComment comment = _service.FirstOrDefaultActive(x => x.Id == id);

            if (comment != null)
            {
                _service.Deactivate(comment);
                result.Succeed = true;
            }
            else
            {

                result.Succeed = false;
            }

            return Json(result);
        }

        private String containingFolder = "CommentImages";

        public ActionResult UpdateComment(int commentId, String content, HttpPostedFileBase image, bool deleteOldImg)
        {
            var result = new AjaxOperationResult();

            var service = this.Service<IPostCommentService>();

            PostComment comment = service.FirstOrDefaultActive(x => x.Id == commentId);

            if (comment != null)
            {
                comment.Comment = content;

                if (image != null)
                {
                    FileUploader uploader = new FileUploader();

                    comment.Image = uploader.UploadImage(image, containingFolder);

                }
                else
                {
                    if (deleteOldImg)
                    {
                        comment.Image = null;
                    }
                }

                service.Update(comment);
                service.Save();
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }


            return Json(result);
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

        private NotificationCustomViewModel PrepareNotificationCustomViewModel(Notification noti)
        {
            NotificationCustomViewModel result = Mapper.Map<NotificationCustomViewModel>(noti);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            var _userService = this.Service<IAspNetUserService>();
            AspNetUser us = _userService.FirstOrDefaultActive(u => u.Id.Equals(noti.FromUserId));
            if (us != null)
            {
                result.Avatar = us.AvatarImage;
            }
            else
            {
                result.Avatar = "";
            }

            return result;

        }
    }
}