using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teek.Models;

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class PostController : BaseController
    {
        private String systemError = "An error has occured!";

        private String userImagePath = "UserImage\\CuongPK";

        [HttpPost]
        public ActionResult ShowAllPost(String currentUserId) {
            var postService = this.Service<IPostService>();

            List<Post> postList = null;

            ResponseModel<List<PostOveralViewModel>> response = null;

            try {
                postList = postService.GetAll().ToList<Post>();

                List<PostOveralViewModel> result = Mapper.Map<List<PostOveralViewModel>>(postList);

                foreach(var p in result)
                {
                    PreparePostOveralData(p, currentUserId);
                }

                response = new ResponseModel<List<PostOveralViewModel>>(true,"Post list loaded!", null,result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<PostOveralViewModel>>.CreateErrorResponse("Post list failed to load!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowPostDetail(int postId, String currentUserId) {
            var postService = this.Service<IPostService>();

            Post post = null;

            ResponseModel<PostDetailViewModel> response = null;

            try {
                post = postService.GetPostById(postId);

                PostOveralViewModel overal = Mapper.Map<PostOveralViewModel>(post);

                PreparePostOveralData(overal,currentUserId);

                PostDetailViewModel result = PreparePostDetailData(overal);

                response = new ResponseModel<PostDetailViewModel>(true,"Post detail has been loaded!",null,result);

            } catch (Exception) {
                response = ResponseModel<PostDetailViewModel>.CreateErrorResponse("Post failed to load!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Post(PostUploadViewModel model) {
            var service = this.Service<IPostService>();

            PostOveralViewModel result = null;

            ResponseModel<PostViewModel> response = null;

            try {


                Post post = Mapper.Map<Post>(model);

                post = service.CreatePost(post);

                result = Mapper.Map<PostOveralViewModel>(post);

                if (model.UploadImage != null) {
                    FileUploader uploader = new FileUploader();

                    result.Image = uploader.UploadImage(model.UploadImage, "UserImage");
                }

                PreparePostOveralData(result,post.UserId);

                response = new ResponseModel<PostViewModel>(true, "Post created", null, result);
            } catch (Exception) {
                response = ResponseModel<PostViewModel>.CreateErrorResponse("Post failed!",systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult EditPost(PostUploadViewModel model, bool imageChanged)
        {
            var service = this.Service<IPostService>();

            ResponseModel<PostOveralViewModel> response = null;

            try
            {
                Post post = Mapper.Map<Post>(model);

                if (imageChanged)
                {
                    if (model.UploadImage != null)
                    {
                        FileUploader uploader = new FileUploader();

                        post.Image = uploader.UploadImage(model.UploadImage, userImagePath);
                    }
                    else {
                        post.Image = null;
                    }
                }

                post = service.EditPost(post, imageChanged);

                PostOveralViewModel result = Mapper.Map<PostOveralViewModel>(post);

                PreparePostOveralData(result, post.UserId);

                response = new ResponseModel<PostOveralViewModel>(true, "Your post has been edited!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<PostOveralViewModel>.CreateErrorResponse("Post edit failed!", systemError);
            }

            return Json(response);
        }


        public void PreparePostOveralData(PostOveralViewModel p, String currentUserId) {

            var likeService = this.Service<ILikeService>();

            var commentService = this.Service<IPostCommentService>();

            var userService = this.Service<IAspNetUserService>();

            p.UserName = userService.FindUserName(p.UserId);

            List<Like> likeList = likeService.GetLikeListByPostId(p.Id).ToList();

            p.LikeCount = likeList.Count();

            foreach (var like in likeList) {
                if (like.UserId==currentUserId) {
                    p.Liked = true;
                }
            }

            p.CommentCount = commentService.GetCommentListByPostId(p.Id).Count();

            

            p.CreateDateStrings();
        }

        public PostDetailViewModel PreparePostDetailData(PostOveralViewModel post) {

            var likeService = this.Service<ILikeService>();

            var commentService = this.Service<IPostCommentService>();

            PostDetailViewModel result = new PostDetailViewModel();

            result.Post = post;

            List<PostComment> commentList = commentService.GetCommentListByPostId(post.Id).ToList<PostComment>();
            List<PostCommentDetailViewModel> commentListResult = Mapper.Map<List<PostCommentDetailViewModel>>(commentList);
            foreach (var c in commentListResult) {
                PreparePostCommentDetailViewModel(c);
            }
            result.CommentList = commentListResult;

            List<Like> likeList = likeService.GetLikeListByPostId(post.Id).ToList<Like>();
            List<LikeDetailViewModel> likeListResult = Mapper.Map<List<LikeDetailViewModel>>(likeList);
            foreach(var l in likeListResult)
            {
                PrepareLikeDetailViewModel(l);
            }

            result.LikeList = likeListResult;

            return result;


        }

        public void PrepareLikeDetailViewModel(LikeDetailViewModel l) {
            var userService = this.Service<IAspNetUserService>();

            l.LikedUserName = userService.FindUserName(l.UserId);

            l.CreateDateString = l.CreateDate.ToString();
            
        }

        public void PreparePostCommentDetailViewModel(PostCommentDetailViewModel p)
        {
            var userService = this.Service<IAspNetUserService>();

            p.CommentedUserName = userService.FindUserName(p.UserId);

            p.CreateDateString = p.CreateDate.ToString();

        }

        //private bool ImageCompareArray(Bitmap firstImage, Bitmap secondImage)
        //{
        //    bool flag = true;
        //    string firstPixel;
        //    string secondPixel;
        //    if (firstImage.Width == secondImage.Width
        //          && firstImage.Height == secondImage.Height)
        //    {
        //        for (int i = 0; i < firstImage.Width; i++)
        //        {
        //            for (int j = 0; j < firstImage.Height; j++)
        //            {
        //                firstPixel = firstImage.GetPixel(i, j).ToString();
        //                secondPixel = secondImage.GetPixel(i, j).ToString();
        //                if (firstPixel != secondPixel)
        //                {
        //                    flag = false;
        //                    break;
        //                }
        //            }
        //        }

        //        if (flag == false)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}