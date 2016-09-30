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
    public class PostController : BaseController
    {
        private String systemError = "An error has occured!";

        [HttpGet]
        public ActionResult ShowAllPost() {
            var postService = this.Service<IPostService>();

            List<Post> postList = null;

            ResponseModel<List<PostOveralViewModel>> response = null;

            try {
                postList = postService.GetAll().ToList<Post>();

                List<PostOveralViewModel> result = Mapper.Map<List<PostOveralViewModel>>(postList);

                foreach(var p in result)
                {
                    PreparePostOveralData(p);
                }

                response = new ResponseModel<List<PostOveralViewModel>>(true,"Post list loaded!", null,result);
            }
            catch (Exception e)
            {
                response = ResponseModel<List<PostOveralViewModel>>.CreateErrorResponse("Post list failed to load!", systemError);
            }

            return Json(response,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowPostDetail(int postId) {
            var postService = this.Service<IPostService>();

            Post post = null;

            ResponseModel<PostDetailViewModel> response = null;

            try {
                post = postService.GetPostById(postId);

                PostOveralViewModel overal = Mapper.Map<PostOveralViewModel>(post);

                PreparePostOveralData(overal);

                PostDetailViewModel result = PreparePostDetailData(overal);

                response = new ResponseModel<PostDetailViewModel>(true,"Post detail has been loaded!",null,result);

            } catch (Exception e) {
                response = ResponseModel<PostDetailViewModel>.CreateErrorResponse("Post failed to load!", systemError);
            }

            return Json(response);
        }



        public void PreparePostOveralData(PostOveralViewModel p) {

            var likeService = this.Service<ILikeService>();

            var commentService = this.Service<IPostCommentService>();

            var userService = this.Service<IAspNetUserService>();

            p.UserName = userService.FindUserName(p.UserId);

            p.LikeCount = likeService.GetLikeListByPostId(p.Id).Count();

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
    }
}