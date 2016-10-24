using Microsoft.AspNet.Identity;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class PostController : BaseController
    {
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreatePost(PostViewModel model, String sportSelect, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            var result = new AjaxOperationResult();
            var _postService = this.Service<IPostService>();
            var post = new Post();
            int ImageNumber = 0;
            bool hasText = false; 
            post.Active = true;
            post.CreateDate = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            
            
            if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
            {
                if (uploadImages.ToList().Count == 1)
                {
                    ImageNumber = 1;
                }else
                {
                    ImageNumber = 2;
                }
                var _postImageService = this.Service<IPostImageService>();
                _postImageService.saveImage(post.Id, uploadImages);
            }
            if (!String.IsNullOrEmpty(model.PostContent))
            {
                hasText = true;
            }

            post.PostContent = model.PostContent;
            if (ImageNumber == 0 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextOnly;
            }
            else if(ImageNumber == 1 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextAndImage;
            }
            else if(ImageNumber == 2 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextAndMultiImages;
            }
            else if(ImageNumber == 1 && !hasText)
            {
                post.ContentType = (int)ContentPostType.ImageOnly;
            }
            else if(ImageNumber == 2 && !hasText)
            {
                post.ContentType = (int)ContentPostType.MultiImages;
            }
            _postService.Create(post);
            if (!String.IsNullOrEmpty(sportSelect))
            {
                string[] sportId = sportSelect.Split(',');
                if (sportId != null)
                {
                    var _postSport = this.Service<IPostSportService>();
                    var postSport = new PostSport();
                    foreach (var item in sportId)
                    {
                        if (!item.Equals(""))
                        {
                            postSport.PostId = post.Id;
                            postSport.SportId = Int32.Parse(item);
                            _postSport.Create(postSport);
                        }
                    }

                }
            }

            return Json(result);

        }
    }
}