using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
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

                Notification noti = notiService.SaveNoti(user.Id, "Comment", commentedUser.UserName + " đã bình luận về bài viết của bạn", 1, post.Id, null);

                PostCommentDetailViewModel result = PreparePostCommentDetailViewModel(comment);

                response = new ResponseModel<PostCommentDetailViewModel>(true, "Bình luận thành công", null, result);

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
    }
}