using HenchmenWeb.Models.Notifications;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Notifications;
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

            var sportService = this.Service<ISportService>();

            var userService = this.Service<IAspNetUserService>();

            var likeService = this.Service<ILikeService>();

            var commentService = this.Service<IPostCommentService>();

            List<Post> postList = null;

            ResponseModel<List<PostOveralViewModel>> response = null;

            try
            {
                postList = postService.GetActive(p => p.UserId == currentUserId ||
                p.AspNetUser.Follows.Where(f => f.FollowerId == currentUserId && f.Active).ToList().Count > 0 ||
                p.Group.GroupMembers.Where(g => g.UserId == currentUserId && g.Active).ToList().Count > 0).ToList();



                //Calculate rank 

                List<AspNetUser> users = userService.GetActive(p => p.Follows.Where(f => (f.UserId == currentUserId ||
                f.FollowerId == currentUserId) && f.Active).ToList().Count > 0).ToList();
                List<AspNetUserRelationViewModel> listUser = Mapper.Map<List<AspNetUserRelationViewModel>>(users);
                foreach (var user in listUser)
                {
                    var totalOfLikeFromUser = likeService.GetActive(p => p.UserId == currentUserId && p.Post.UserId == user.Id).ToList().Count;
                    var totalOfCommentFromUser = commentService.GetActive(p => p.UserId == currentUserId && p.Post.UserId == user.Id).ToList().Count;
                    user.relationScore = totalOfCommentFromUser + totalOfLikeFromUser;
                }


                List<PostGeneralViewModel> listPostVM = Mapper.Map<List<PostGeneralViewModel>>(postList);

                foreach (var item in listPostVM)
                {
                    PrepareDetailPostData(item, currentUserId);
                }

                foreach (var post in listPostVM)
                {

                    //Cal Relation Score
                    float relaScore = 0;
                    if (post.UserId == currentUserId)
                    {
                        relaScore = 1;
                    }
                    foreach (var user in listUser)
                    {
                        if (user.Id == post.UserId)
                        {
                            relaScore = user.relationScore;
                            break;
                        }
                    }

                    //Cal PostWeight
                    int postWeight = 0;
                    List<Sport> sportList = sportService.GetActive(p => p.Hobbies.Where(f =>
                     f.UserId == currentUserId).ToList().Count > 0).ToList();
                    if (sportList != null)
                    {
                        foreach (var item in post.PostSports)
                        {
                            foreach (var sport in sportList)
                            {
                                if (item.SportId == sport.Id)
                                {
                                    postWeight++;
                                }
                            }
                        }

                    }
                    post.PostWeight = postWeight;

                    //Cal TimeDecay
                    post.TimeDecay = postService.CalculateTimeDecay(post.LatestInteractionTime.Value);
                    //Cal PostRank
                    post.PostRank = (relaScore * (post.PostWeight + 1)) / post.TimeDecay;


                }

                List<PostGeneralViewModel> orderedList = listPostVM.OrderByDescending(p => p.PostRank).Skip(skip).Take(take).ToList();

                List<PostOveralViewModel> result = Mapper.Map<List<PostOveralViewModel>>(orderedList);

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

            var notiService = this.Service<INotificationService>();

            var memberService = this.Service<IGroupMemberService>();

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

                //Notify all group members
                if (post.GroupId != null)
                {
                    List<GroupMember> memberList = GetMemberList(post.GroupId);

                    foreach (var member in memberList)
                    {
                        if (!(member.UserId.Equals(post.UserId)))
                        {
                            Notification noti = notiService.SaveNoti(member.UserId, post.UserId, "Post", member.AspNetUser.FullName + " đã đăng một bài viết", (int)NotificationType.Post, post.Id, null, null);

                            List<string> registrationIds = GetToken(member.UserId);

                            if (registrationIds != null && registrationIds.Count != 0)
                            {
                                NotificationModel notiModel = Mapper.Map<NotificationModel>(PrepareNotificationViewModel(noti));

                                Android.Notify(registrationIds, null, notiModel);
                            }
                        }
                    }
                }
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

        [HttpPost]
        public ActionResult DeletePost(int id)
        {
            var postService = this.Service<IPostService>();

            var imageService = this.Service<IPostImageService>();

            ResponseModel<bool> response = null;

            try
            {
                Post post = postService.FirstOrDefaultActive(x => x.Id == id);

                List<PostImage> imageList = post.PostImages.ToList();

                if (imageList != null)
                {
                    foreach (var img in imageList)
                    {
                        imageService.Delete(img);
                    }

                }

                postService.Deactivate(post);

                response = new ResponseModel<bool>(true, "Xóa bài thành công", null);

            }
            catch (Exception)
            {
                response = ResponseModel<bool>.CreateErrorResponse("Xóa bài thất bại", systemError);

            }
            return Json(response);
        }


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

        private NotificationCustomViewModel PrepareNotificationViewModel(Notification noti)
        {
            NotificationCustomViewModel result = Mapper.Map<NotificationCustomViewModel>(noti);

            result.CreateDateString = result.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");

            result.Avatar = noti.AspNetUser1.AvatarImage;

            return result;

        }

        private List<string> GetToken(String userId)
        {
            var service = this.Service<IFirebaseTokenService>();

            List<FirebaseToken> tokenList = service.Get(x => x.UserId.Equals(userId)).ToList();

            List<string> registrationIds = new List<string>();
            if (tokenList != null)
            {
                foreach (var token in tokenList)
                {
                    registrationIds.Add(token.Token);
                }
            }

            return registrationIds;
        }

        public void PrepareDetailPostData(PostGeneralViewModel p, string curUserId)
        {
            var _postService = this.Service<IPostService>();
            var _likeService = this.Service<ILikeService>();
            var _postCommentService = this.Service<IPostCommentService>();
            var _postSportService = this.Service<IPostSportService>();

            //like
            List<Like> likeList = _likeService.GetLikeListByPostId(p.Id).ToList();
            p.LikeCount = likeList.Count();
            foreach (var item in likeList)
            {
                if (item.UserId == curUserId)
                {
                    p.Liked = true;
                }
                else
                {
                    p.Liked = false;
                }
            }

            //comment
            List<PostComment> postCmtList = _postCommentService.GetCommentListByPostId(p.Id, 0, 3).ToList();
            //p.PostAge = _postService.CalculatePostAge(p.EditDate == null ? p.CreateDate : p.EditDate.Value);
            p.PostAge = _postService.CalculatePostAge(p.CreateDate);
            p.PostComments = Mapper.Map<List<PostCommentDetailViewModel>>(postCmtList);
            p.CommentCount = _postCommentService.GetActive(c => c.PostId == p.Id).ToList().Count();
            foreach (var item in p.PostComments)
            {
                //DateTime dt = DateTime.ParseExact(item.CreateDateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                item.CommentAge = _postCommentService.CalculateCommentAge(item.CreateDate);
            }

            //sport
            List<PostSport> postSportList = _postSportService.GetActive(s => s.PostId == p.Id).ToList();
            p.PostSports = Mapper.Map<List<PostSportDetailViewModel>>(postSportList);
        }

        private List<GroupMember> GetMemberList(int? groupId)
        {
            var service = this.Service<IGroupMemberService>();

            return service.GetActive(x => x.GroupId == groupId).ToList();
        }

    }
}