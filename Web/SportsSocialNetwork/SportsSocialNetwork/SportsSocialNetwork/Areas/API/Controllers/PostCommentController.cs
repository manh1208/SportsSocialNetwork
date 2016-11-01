﻿using HenchmenWeb.Models.Notifications;
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
using Teek.Models;
using static System.Net.WebRequestMethods;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class PostCommentController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        private String userImagePath = "PostComment";

        [HttpPost]
        public ActionResult GetComment(int postId, int skip, int take) {
            var service = this.Service<IPostCommentService>();

            ResponseModel<List<PostCommentDetailViewModel>> response = null;

            try {
                List<PostComment> commentList= service.GetCommentListByPostId(postId,skip,take).ToList();

                List<PostCommentDetailViewModel> result = new List<PostCommentDetailViewModel>();

                foreach (var comment in commentList) {
                    result.Add(PreparePostCommentDetailViewModel(comment));
                }

                response = new ResponseModel<List<PostCommentDetailViewModel>>(true,"Tải bình luận thành công",null, result);


            } catch (Exception) {
                response = ResponseModel<List<PostCommentDetailViewModel>>.CreateErrorResponse("Tải bình luận thất bại", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Comment(int postId, String userId, String content , HttpPostedFileBase image) {
            var service = this.Service<IPostCommentService>();

            var notiService = this.Service<INotificationService>();

            var postService = this.Service<IPostService>();

            var aspNetUserService = this.Service<IAspNetUserService>();

            ResponseModel<PostCommentDetailViewModel> response = null;

            try {
                String commentImage = null;

                if (image != null) {
                    FileUploader uploader = new FileUploader();

                    commentImage = uploader.UploadImage(image, userImagePath);
                }

                PostComment comment = service.Comment(postId, userId, content,commentImage);

                AspNetUser commentedUser = aspNetUserService.FindUser(comment.UserId);

                Post post = postService.GetPostById(comment.PostId);

                post.LatestInteractionTime = DateTime.Now;

                AspNetUser user = postService.GetUserNameOfPost(post.Id);

                if (!(user.Id == commentedUser.Id))
                {
                    Notification noti = notiService.SaveNoti(user.Id, commentedUser.Id, "Comment", commentedUser.FullName + " đã bình luận về bài viết của bạn", int.Parse(NotificationType.Post.ToString("d")), post.Id, null,null);

                    List<string> registrationIds = GetToken(user.Id);

                    //registrationIds.Add("dgizAK4sGBs:APA91bGtyQTwOiAgNHE_mIYCZhP0pIqLCUvDzuf29otcT214jdtN2e9D6iUPg3cbYvljKbbRJj5z7uaTLEn1WeUam3cnFqzU1E74AAZ7V82JUlvUbS77mM42xHZJ5DifojXEv3JPNEXQ");

                    if (registrationIds != null && registrationIds.Count != 0)
                    {
                        NotificationModel model = Mapper.Map<NotificationModel>(PrepareNotificationViewModel(noti));

                        Android.Notify(registrationIds, null, model);
                    }

                }

                PostCommentDetailViewModel result = PreparePostCommentDetailViewModel(comment);

                response = new ResponseModel<PostCommentDetailViewModel>(true, "Bình luận thành công", null, result);

            } catch (Exception) {
                response = ResponseModel<PostCommentDetailViewModel>.CreateErrorResponse("Bình luận thất bại", systemError);
            }
            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            var service = this.Service<IPostCommentService>();

            ResponseModel<bool> response = null;

            try {
                PostComment comment = service.FirstOrDefaultActive(x => x.Id == id);

                if (comment != null)
                {
                    service.Deactivate(comment);

                    response = new ResponseModel<bool>(true, "Đã xóa bài viết", null);
                }
                else
                {
                    response = ResponseModel<bool>.CreateErrorResponse("Không thể xóa bài viết", systemError);
                }
            } catch(Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Không thể xóa bài viết", systemError);
            }
            return Json(response);
        }

        private PostCommentDetailViewModel PreparePostCommentDetailViewModel(PostComment comment) {
            PostCommentDetailViewModel result = Mapper.Map<PostCommentDetailViewModel>(comment);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            return result;
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