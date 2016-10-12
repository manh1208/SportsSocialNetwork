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
using static System.Net.WebRequestMethods;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class PostCommentController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        private String userImagePath = "UserImage\\CuongPK";

        [HttpPost]
        public ActionResult GetComment(int postId, int skip, int take) {
            var service = this.Service<IPostCommentService>();

            ResponseModel<List<PostCommentDetailViewModel>> response = null;

            try {
                List<PostComment> commentList= service.GetCommentListByPostId(postId,skip,take).ToList();

                List<PostCommentDetailViewModel> result = Mapper.Map<List<PostCommentDetailViewModel>>(commentList);

                foreach(var comment in result)
                {
                    PreparePostCommentDetailViewModel(comment);
                }

                response = new ResponseModel<List<PostCommentDetailViewModel>>(true,"Comment list loaded",null, result);


            } catch (Exception) {
                response = ResponseModel<List<PostCommentDetailViewModel>>.CreateErrorResponse("Failed to load comments", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Comment(int postId, String userId, String content , HttpPostedFileBase image) {
            var service = this.Service<IPostCommentService>();

            ResponseModel<PostCommentDetailViewModel> response = null;

            try {
                String commentImage = null;

                if (image != null) {
                    FileUploader uploader = new FileUploader();

                    commentImage = uploader.UploadImage(image, userImagePath);
                }

                PostComment comment = service.Comment(postId, userId, content,commentImage);


                PostCommentDetailViewModel result = Mapper.Map<PostCommentDetailViewModel>(comment);

                PreparePostCommentDetailViewModel(result);

                response = new ResponseModel<PostCommentDetailViewModel>(true, "Commented successfully", null, result);

            } catch (Exception) {
                response = ResponseModel<PostCommentDetailViewModel>.CreateErrorResponse("Failed to comment",systemError);
            }
            return Json(response);
        }

        public void PreparePostCommentDetailViewModel(PostCommentDetailViewModel p)
        {
            var userService = this.Service<IAspNetUserService>();

            p.CreateDateString = p.CreateDate.ToString();

        }
    }
}