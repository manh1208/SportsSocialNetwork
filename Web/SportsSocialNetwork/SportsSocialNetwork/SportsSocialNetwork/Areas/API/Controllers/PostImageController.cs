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

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class PostImageController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult GetAllPostImage(String userId)
        {
            var userService = this.Service<IAspNetUserService>();

            var imageService = this.Service<IPostImageService>();

            ResponseModel<List<PostImageViewModel>> response = null;

            try {
                AspNetUser user = userService.FirstOrDefaultActive(x => x.Id == userId);

                List<Post> postList = user.Posts.ToList();

                List<PostImage> imageList = new List<PostImage>();

                foreach(var post in postList)
                {
                    foreach(var img in imageService.GetAllPostImage(post.Id))
                    {
                        imageList.Add(img);
                    }
                }

                List<PostImageViewModel> result = Mapper.Map<List<PostImageViewModel>>(imageList);

                response = new ResponseModel<List<PostImageViewModel>>(true, "Danh sách hình:", null,result);
            } catch (Exception) {
                response = ResponseModel<List<PostImageViewModel>>.CreateErrorResponse("Không thể tải hình", systemError);
            }

            return Json(response);
        }
    }
}