using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
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
        public ActionResult ShowAllUserPost(String userId, String currentUserId, int skip, int take)
        {
            var postService = this.Service<IPostService>();

            List<Post> postList = null;

            ResponseModel<List<PostOveralViewModel>> response = null;

            try
            {
                postList = postService.GetAllPostOfUser(userId, skip, take).ToList<Post>();

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

                PostDetailViewModel result = PreparePostDetailData(overal, skip, take);

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
                if (model.PostContent == null)
                {
                    post.PostContent = "";
                }
                post.ContentType = GetPostType(model);

                if (post.ContentType != int.Parse(ContentPostType.TextOnly.ToString("d")))
                {
                    FileUploader uploader = new FileUploader();

                    foreach (var img in model.UploadImage)
                    {
                        PostImage image = new PostImage();

                        image.Image = uploader.UploadImage(img, userImagePath);

                        post.PostImages.Add(image);
                    }
                }

                post = service.CreatePost(post);

                //Missing post sport

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
        public ActionResult EditPost(PostUploadViewModel model)
        {
            var postService = this.Service<IPostService>();

            var postImageService = this.Service<IPostImageService>();

            ResponseModel<PostOveralViewModel> response = null;

            try
            {
                Post post = postService.FirstOrDefaultActive(x => x.Id == model.Id);

                post.ContentType = GetPostType(model);

                if (model.UploadImage != null)
                {
                    postImageService.saveImage(post.Id, model.UploadImage);
                }

                if (model.DeleteImage != null && model.DeleteImage.Count > 0)
                {
                    foreach (var delete in model.DeleteImage)
                    {
                        PostImage img = postImageService.FirstOrDefaultActive(x => x.Id == delete);
                        postImageService.Delete(img);
                    }
                }

                post.PostContent = model.PostContent;

                post.EditDate = DateTime.Now;

                postService.Update(post);

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

            p.CreateDateString = p.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            if (!p.EditDate.ToString().Equals("01/01/0001 12:00:00 SA"))
            {
                p.EditDateString = p.EditDate.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        public PostDetailViewModel PreparePostDetailData(PostOveralViewModel post, int skip, int take)
        {

            var likeService = this.Service<ILikeService>();

            var commentService = this.Service<IPostCommentService>();

            PostDetailViewModel result = new PostDetailViewModel();

            result.Post = post;

            List<PostComment> commentList = commentService.GetCommentListByPostId(post.Id, skip, take).ToList<PostComment>();
            List<PostCommentDetailViewModel> commentListResult = new List<PostCommentDetailViewModel>();
            foreach (var comment in commentList)
            {
                commentListResult.Add(PreparePostCommentDetailViewModel(comment));
            }
            result.CommentList = commentListResult;
            return result;
        }

        //private int GetPostType(PostUploadViewModel model)
        //{
        //    int contentType = 0;

        //    bool hasText = false;

        //    if (model.PostContent == null)
        //    {
        //        hasText = false;
        //    }
        //    else
        //    {
        //        hasText = true;
        //    }


        //    if (model.UploadImage==null && hasText)
        //    {
        //        contentType = int.Parse(ContentPostType.TextOnly.ToString("d"));
        //    }
        //    else if (model.UploadImage.Count == 1 && hasText)
        //    {
        //        contentType = int.Parse(ContentPostType.TextAndImage.ToString("d"));
        //    }
        //    else if (model.UploadImage.Count > 1 && hasText)
        //    {
        //        contentType = int.Parse(ContentPostType.TextAndMultiImages.ToString("d"));
        //    }
        //    else if (model.UploadImage.Count > 1 && !hasText)
        //    {
        //        contentType = int.Parse(ContentPostType.MultiImages.ToString("d"));
        //    }
        //    else if (model.UploadImage.Count == 1 && !hasText) {
        //        contentType = int.Parse(ContentPostType.ImageOnly.ToString("d"));
        //    }

        //    return contentType;
        //}

        private int GetPostType(PostUploadViewModel model)
        {
            var service = this.Service<IPostService>();

            int contentType = 0;

            bool hasText = false;

            int numberOfImages = 0;

            if (model.PostContent == null)
            {
                hasText = false;
            }
            else
            {
                hasText = true;
            }

            Post post = service.FirstOrDefaultActive(x => x.Id == model.Id);
            if (post != null)
            {
                if (post.PostImages != null)
                {
                    numberOfImages = post.PostImages.Count();
                }

            }

            if (model.UploadImage != null && model.UploadImage.Count() > 0)
            {
                numberOfImages = numberOfImages + model.UploadImage.Count();
            }
            if (model.DeleteImage != null && model.DeleteImage.Count() > 0)
            {
                numberOfImages = numberOfImages - model.DeleteImage.Count();
            }

            if (numberOfImages == 0 && hasText)
            {
                contentType = int.Parse(ContentPostType.TextOnly.ToString("d"));
            }
            else if (numberOfImages == 1 && hasText)
            {
                contentType = int.Parse(ContentPostType.TextAndImage.ToString("d"));
            }
            else if (numberOfImages > 1 && hasText)
            {
                contentType = int.Parse(ContentPostType.TextAndMultiImages.ToString("d"));
            }
            else if (numberOfImages > 1 && !hasText)
            {
                contentType = int.Parse(ContentPostType.MultiImages.ToString("d"));
            }
            else if (numberOfImages == 1 && !hasText)
            {
                contentType = int.Parse(ContentPostType.ImageOnly.ToString("d"));
            }

            return contentType;
        }

        private PostCommentDetailViewModel PreparePostCommentDetailViewModel(PostComment comment)
        {
            PostCommentDetailViewModel result = Mapper.Map<PostCommentDetailViewModel>(comment);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            return result;
        }
    }
}