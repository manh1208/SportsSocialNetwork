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
using static System.Net.WebRequestMethods;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class PostCommentController : BaseController
    {
        private String systemError = "An error has occured!";

        [HttpPost]
        public ActionResult Comment(int postId, String userId, String content ) {
            var service = this.Service<IPostCommentService>();

            ResponseModel<PostCommentViewModel> response = null;

            try {
                PostComment comment = service.Comment(postId, userId, content);

                PostCommentDetailViewModel result = Mapper.Map<PostCommentDetailViewModel>(comment);

                PreparePostCommentDetailViewModel(result);

                response = new ResponseModel<PostCommentViewModel>(true, "Commented successfully", null, result);

            } catch (Exception e) {
                response = ResponseModel<PostCommentViewModel>.CreateErrorResponse("Failed to comment",systemError);
            }
            return Json(response);
        }

        public void PreparePostCommentDetailViewModel(PostCommentDetailViewModel p)
        {
            var userService = this.Service<IAspNetUserService>();

            p.CommentedUserName = userService.FindUserName(p.UserId);

            p.CreateDateString = p.CreateDate.ToString();

        }
    }
}