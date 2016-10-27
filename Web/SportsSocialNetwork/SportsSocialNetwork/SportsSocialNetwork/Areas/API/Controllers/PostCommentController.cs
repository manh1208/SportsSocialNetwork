using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
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

                AspNetUser user = postService.GetUserNameOfPost(post.Id);

                if (!(user.Id == commentedUser.Id))
                {
                    Notification noti = notiService.SaveNoti(user.Id, commentedUser.Id, "Comment", commentedUser.FullName + " đã bình luận về bài viết của bạn", int.Parse(NotificationType.Post.ToString("d")), post.Id, null,null);

                }

                PostCommentDetailViewModel result = PreparePostCommentDetailViewModel(comment);

                response = new ResponseModel<PostCommentDetailViewModel>(true, "Bình luận thành công", null, result);

            } catch (Exception) {
                response = ResponseModel<PostCommentDetailViewModel>.CreateErrorResponse("Bình luận thất bại", systemError);
            }
            return Json(response);
        }

        private PostCommentDetailViewModel PreparePostCommentDetailViewModel(PostComment comment) {
            PostCommentDetailViewModel result = Mapper.Map<PostCommentDetailViewModel>(comment);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            return result;
        }
    }
}