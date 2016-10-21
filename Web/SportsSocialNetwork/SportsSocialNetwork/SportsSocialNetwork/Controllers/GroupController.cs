using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class GroupController : BaseController
    {
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getAllPost(string userId)
        {
            var result = new AjaxOperationResult<IEnumerable<PostGeneralViewModel>>();
            if (!String.IsNullOrEmpty(userId))
            {
                var _postService = this.Service<IPostService>();
                var _postImageService = this.Service<IPostImageService>();
                List<PostGeneralViewModel> listPostVM = new List<PostGeneralViewModel>();
                List<Post> postList = _postService.GetActive(p => p.UserId.Equals(userId)).ToList();

                foreach (var item in postList)
                {
                    PostGeneralViewModel model = Mapper.Map<PostGeneralViewModel>(item);
                    if(item.EditDate != null)
                    {
                        model.PostAge = _postService.CalculatePostAge(item.EditDate.Value);
                    }
                    else
                    {
                        model.PostAge = _postService.CalculatePostAge(item.CreateDate);
                    }
                    
                    listPostVM.Add(model);
                }
                
                result.AdditionalData = listPostVM;
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