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
        private String systemError = "Đã có lỗi xảy ra!";

        private String userImagePath = "Post";

        [HttpPost]
        public ActionResult ShowAllPost(String currentUserId, int skip, int take)
        {
            var postService = this.Service<IPostService>();

            List<Post> postList = null;

            ResponseModel<List<PostOveralViewModel>> response = null;

            try
            {
                postList = postService.GetAll(skip, take).ToList<Post>();

                List<PostOveralViewModel> result = Mapper.Map<List<PostOveralViewModel>>(postList);

                foreach (var p in result)
                {
                    PreparePostOveralData(p, currentUserId);
                }

                response = new ResponseModel<List<PostOveralViewModel>>(true, "Tải bài viết thành công!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<PostOveralViewModel>>.CreateErrorResponse("Tải bài viết thất bại!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowAllGroupPosts(int groupId, String currentUserId, int skip, int take)
        {
            var postService = this.Service<IPostService>();

            List<Post> postList = null;

            ResponseModel<List<PostOveralViewModel>> response = null;

            try
            {
                postList = postService.GetAllPostsOfGroup(groupId, skip, take).ToList<Post>();

                List<PostOveralViewModel> result = Mapper.Map<List<PostOveralViewModel>>(postList);

                foreach (var p in result)
                {
                    PreparePostOveralData(p, currentUserId);
                }

                response = new ResponseModel<List<PostOveralViewModel>>(true, "Tải bài viết thành công!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<PostOveralViewModel>>.CreateErrorResponse("Tải bài viết thất bại!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowPostDetail(int postId, String currentUserId, int skip, int take)
        {
            var postService = this.Service<IPostService>();

            Post post = null;

            ResponseModel<PostDetailViewModel> response = null;

            try
            {
                post = postService.GetPostById(postId);

                PostOveralViewModel overal = Mapper.Map<PostOveralViewModel>(post);

                PreparePostOveralData(overal, currentUserId);

                PostDetailViewModel result = PreparePostDetailData(overal,skip,take);

                response = new ResponseModel<PostDetailViewModel>(true, "Chi tiết bài viết:", null, result);

            }
            catch (Exception)
            {
                response = ResponseModel<PostDetailViewModel>.CreateErrorResponse("Tải bài viết thất bại!", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult Post(PostUploadViewModel model)
        {
            var service = this.Service<IPostService>();

            var aspNetUserService = this.Service<IAspNetUserService>();

            PostOveralViewModel result = null;

            ResponseModel<PostOveralViewModel> response = null;

            try
            {


                Post post = Mapper.Map<Post>(model);

                //if (model.UploadImage != null)
                //{
                //    FileUploader uploader = new FileUploader();

                //    post.Image = uploader.UploadImage(model.UploadImage, "UserImage");
                //}

                post = service.CreatePost(post);

                result = Mapper.Map<PostOveralViewModel>(post);

                result.AspNetUser = Mapper.Map<AspNetUserSimpleModel>(aspNetUserService.FindUser(result.UserId));

                PreparePostOveralData(result, post.UserId);

                response = new ResponseModel<PostOveralViewModel>(true, "Đăng bài thành công!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<PostOveralViewModel>.CreateErrorResponse("Đăng bài thất bại!", systemError);
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

                //if (imageChanged)
                //{
                //    if (model.UploadImage != null)
                //    {
                //        FileUploader uploader = new FileUploader();

                //        post.Image = uploader.UploadImage(model.UploadImage, userImagePath);
                //    }
                //    else
                //    {
                //        post.Image = null;
                //    }
                //}

                post = service.EditPost(post, imageChanged);

                PostOveralViewModel result = Mapper.Map<PostOveralViewModel>(post);

                PreparePostOveralData(result, post.UserId);

                response = new ResponseModel<PostOveralViewModel>(true, "Bài viết đã được chỉnh sửa!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<PostOveralViewModel>.CreateErrorResponse("Chỉnh sửa thất bại!", systemError);
            }

            return Json(response);
        }


        public void PreparePostOveralData(PostOveralViewModel p, String currentUserId)
        {

            var likeService = this.Service<ILikeService>();

            var commentService = this.Service<IPostCommentService>();

            List<Like> likeList = likeService.GetLikeListByPostId(p.Id).ToList();

            p.LikeCount = likeList.Count();

            foreach (var like in likeList)
            {
                if (like.UserId == currentUserId)
                {
                    p.Liked = true;
                }
            }

            p.CommentCount = commentService.GetCommentListByPostId(p.Id).Count();



        }

        public PostDetailViewModel PreparePostDetailData(PostOveralViewModel post,int skip, int take)
        {

            var likeService = this.Service<ILikeService>();

            var commentService = this.Service<IPostCommentService>();

            PostDetailViewModel result = new PostDetailViewModel();

            result.Post = post;

            List<PostComment> commentList = commentService.GetCommentListByPostId(post.Id,skip,take).ToList<PostComment>();
            List<PostCommentDetailViewModel> commentListResult = Mapper.Map<List<PostCommentDetailViewModel>>(commentList);
            result.CommentList = commentListResult;
            return result;
        }
    }
}