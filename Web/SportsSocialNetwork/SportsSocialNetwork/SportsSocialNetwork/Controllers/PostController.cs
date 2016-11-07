using HenchmenWeb.Models.Notifications;
using Microsoft.AspNet.Identity;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Notifications;
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
            var result = new AjaxOperationResult<PostGeneralViewModel>();
            var _postService = this.Service<IPostService>();
            var post = new Post();
            int ImageNumber = 0;
            bool hasText = false; 
            post.Active = true;
            post.CreateDate = DateTime.Now;
            post.LatestInteractionTime = post.CreateDate;
            post.UserId = User.Identity.GetUserId();
            
            if(uploadImages != null)
            {
                if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
                {
                    if (uploadImages.ToList().Count == 1)
                    {
                        ImageNumber = 1;
                    }
                    else
                    {
                        ImageNumber = 2;
                    }
                }
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
            _postService.Save();
            if (uploadImages != null)
            {
                if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
                {
                    var _postImageService = this.Service<IPostImageService>();
                    _postImageService.saveImage(post.Id, uploadImages);
                }
            }
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

            result.AdditionalData = Mapper.Map<PostGeneralViewModel>(post);
            return Json(result);

        }

        //create profile post
        public ActionResult CreateProfilePost(string content, String sportSelect, IEnumerable<HttpPostedFileBase> uploadImages, string userId, string profileId)
        {
            var result = new AjaxOperationResult<PostGeneralViewModel>();
            var _postService = this.Service<IPostService>();
            var post = new Post();
            int ImageNumber = 0;
            bool hasText = false;
            post.Active = true;
            post.CreateDate = DateTime.Now;
            post.LatestInteractionTime = DateTime.Now;
            post.UserId = userId;
            post.ProfileId = profileId;

            if (uploadImages != null)
            {
                if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
                {
                    if (uploadImages.ToList().Count == 1)
                    {
                        ImageNumber = 1;
                    }
                    else
                    {
                        ImageNumber = 2;
                    }
                }
            }

            if (!String.IsNullOrEmpty(content))
            {
                hasText = true;
            }

            post.PostContent = content;
            if (ImageNumber == 0 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextOnly;
            }
            else if (ImageNumber == 1 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextAndImage;
            }
            else if (ImageNumber == 2 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextAndMultiImages;
            }
            else if (ImageNumber == 1 && !hasText)
            {
                post.ContentType = (int)ContentPostType.ImageOnly;
            }
            else if (ImageNumber == 2 && !hasText)
            {
                post.ContentType = (int)ContentPostType.MultiImages;
            }
            _postService.Create(post);
            _postService.Save();
            if (uploadImages != null)
            {
                if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
                {
                    var _postImageService = this.Service<IPostImageService>();
                    _postImageService.saveImage(post.Id, uploadImages);
                }
            }
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

            result.AdditionalData = Mapper.Map<PostGeneralViewModel>(post);
            return Json(result);

        }

        //create group post
        public ActionResult CreateGroupPost(string content, String sportSelect, IEnumerable<HttpPostedFileBase> uploadImages, int groupId)
        {
            var result = new AjaxOperationResult<PostGeneralViewModel>();
            var _postService = this.Service<IPostService>();
            var _notiService = this.Service<INotificationService>();
            var _memberService = this.Service<IGroupMemberService>();
            var _userService = this.Service<IAspNetUserService>();
            var post = new Post();
            int ImageNumber = 0;
            bool hasText = false;
            post.Active = true;
            post.CreateDate = DateTime.Now;
            post.LatestInteractionTime = post.CreateDate;
            post.UserId = User.Identity.GetUserId();
            post.GroupId = groupId;

            if (uploadImages != null)
            {
                if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
                {
                    if (uploadImages.ToList().Count == 1)
                    {
                        ImageNumber = 1;
                    }
                    else
                    {
                        ImageNumber = 2;
                    }
                }
            }

            if (!String.IsNullOrEmpty(content))
            {
                hasText = true;
            }

            post.PostContent = content;
            if (ImageNumber == 0 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextOnly;
            }
            else if (ImageNumber == 1 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextAndImage;
            }
            else if (ImageNumber == 2 && hasText)
            {
                post.ContentType = (int)ContentPostType.TextAndMultiImages;
            }
            else if (ImageNumber == 1 && !hasText)
            {
                post.ContentType = (int)ContentPostType.ImageOnly;
            }
            else if (ImageNumber == 2 && !hasText)
            {
                post.ContentType = (int)ContentPostType.MultiImages;
            }
            _postService.Create(post);
            _postService.Save();

            if (post.GroupId != null)
            {
                List<GroupMember> memberList = _memberService.GetActive(x => x.GroupId == post.GroupId).ToList();

                AspNetUser postedUser = _userService.FirstOrDefaultActive(x => x.Id.Equals(post.UserId));

                foreach (var member in memberList)
                {
                    if (!(member.UserId.Equals(post.UserId)))
                    {
                        Notification noti = _notiService.SaveNoti(member.UserId, post.UserId, "Post", postedUser.FullName + "đã đăng một bài viết", (int)NotificationType.Post, post.Id, null, null);

                        List<string> registrationIds = GetToken(member.UserId);

                        if (registrationIds != null && registrationIds.Count != 0)
                        {
                            NotificationModel notiModel = Mapper.Map<NotificationModel>(PrepareNotificationCustomViewModel(noti));

                            Android.Notify(registrationIds, null, notiModel);
                        }
                    }
                }

            }

            if (uploadImages != null)
            {
                if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
                {
                    var _postImageService = this.Service<IPostImageService>();
                    _postImageService.saveImage(post.Id, uploadImages);
                }
            }
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

            result.AdditionalData = Mapper.Map<PostGeneralViewModel>(post);
            return Json(result);

        }

        public ActionResult DeletePost(int id)
        {
            var result = new AjaxOperationResult();
            var _postService = this.Service<IPostService>();

            var _postImageService = this.Service<IPostImageService>();

            Post post = _postService.FirstOrDefaultActive(x => x.Id == id);

            if (post != null)
            {
                List<PostImage> imageList = _postImageService.GetActive(x => x.PostId == post.Id).ToList();

                if (imageList != null)
                {
                    for (int i = 0; i < imageList.Count; i++)
                    {
                        _postImageService.Delete(imageList[i]);
                    }
                }

                _postService.Deactivate(post);
                result.Succeed = true;
            }else
            {

                result.Succeed = false;
            }

            return Json(result);
        }

        public ActionResult LoadSavedPost(String postId)
        {
            int id = Int32.Parse(postId);
            var result = new AjaxOperationResult<PostGeneralViewModel>();
            var _postService = this.Service<IPostService>();
            Post tmp = _postService.FirstOrDefaultActive(p => p.Id == id);
            if (tmp != null)
            {
                var _userService = this.Service<IAspNetUserService>();
                PostGeneralViewModel postGeneral = Mapper.Map<PostGeneralViewModel>(tmp);
                var user = Mapper.Map<AspNetUserSimpleModel>(_userService.Get<string>(postGeneral.UserId));
                postGeneral.AspNetUser = user;
                PrepareDetailPostData(postGeneral, User.Identity.GetUserId());
                result.AdditionalData = postGeneral;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdatePost(String postEditId, String PostContentEdit, String sportSelectEdit, List<HttpPostedFileBase> uploadImages, List<int> deleteImages, List<int> notDeleteImages)
        {
            int postId = Int32.Parse(postEditId);
            var result = new AjaxOperationResult();

            var _postService = this.Service<IPostService>();
            var _postSport = this.Service<IPostSportService>();
            var _postImageService = this.Service<IPostImageService>();

            int ImageNumber = 0;
            bool hasText = false;
            Post post = _postService.FirstOrDefaultActive(x => x.Id == postId);

            if (post != null)
            {
                post.EditDate = DateTime.Now;
                post.LatestInteractionTime = post.EditDate;
                if (deleteImages != null && deleteImages.Count > 0)
                {
                    foreach (var delete in deleteImages)
                    {
                        PostImage img =  _postImageService.FirstOrDefaultActive(x => x.Id == delete);
                        _postImageService.Delete(img);
                    }
                }

                var uploadImgNum = 0;
                var notDeleteImgNum = 0;
                if (uploadImages != null)
                {
                    uploadImgNum = uploadImages.Count;
                    _postImageService.saveImage(post.Id, uploadImages);
                }
                if (notDeleteImages != null)
                {
                    notDeleteImgNum = notDeleteImages.Count;
                }
                var totalImg = uploadImgNum + notDeleteImgNum;
                if(totalImg == 1)
                {
                    ImageNumber = 1;
                }else if(totalImg > 1)
                {
                    ImageNumber = 2;
                }

                if (!String.IsNullOrEmpty(PostContentEdit))
                {
                    hasText = true;
                }
                post.PostContent = PostContentEdit;
                if (ImageNumber == 0 && hasText)
                {
                    post.ContentType = (int)ContentPostType.TextOnly;
                }
                else if (ImageNumber == 1 && hasText)
                {
                    post.ContentType = (int)ContentPostType.TextAndImage;
                }
                else if (ImageNumber == 2 && hasText)
                {
                    post.ContentType = (int)ContentPostType.TextAndMultiImages;
                }
                else if (ImageNumber == 1 && !hasText)
                {
                    post.ContentType = (int)ContentPostType.ImageOnly;
                }
                else if (ImageNumber == 2 && !hasText)
                {
                    post.ContentType = (int)ContentPostType.MultiImages;
                }

                List<PostSport> sportList = _postSport.Get(x => x.PostId == postId).ToList();
                if (sportList != null)
                {
                    foreach (var sport in sportList)
                    {

                        _postSport.Delete(sport);
                    }
                }
                if (!String.IsNullOrEmpty(sportSelectEdit))
                {
                    string[] sportId = sportSelectEdit.Split(',');
                    if (sportId != null)
                    {
                        
                        foreach (var item in sportId)
                        {
                            if (!item.Equals(""))
                            {
                                PostSport postSport = new PostSport();
                                postSport.PostId = post.Id;
                                postSport.SportId = Int32.Parse(item);
                                _postSport.Create(postSport);
                                
                            }
                        }
                    }
                }
                _postService.Update(post);
                _postService.Save();
                result.Succeed = true;
            }else
            {
                result.Succeed = false;
            }
            return Json(result);
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
            p.PostAge = _postService.CalculatePostAge(p.EditDate == null ? p.CreateDate : p.EditDate.Value);
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

        public ActionResult UpdatePost(PostViewModel model, String sportSelect, List<HttpPostedFileBase> uploadImages, List<int> deleteImages)
        {
            var result = new AjaxOperationResult();

            var _postService = this.Service<IPostService>();

            var _postImageService = this.Service<IPostImageService>();

            int ImageNumber = 0;
            bool hasText = false;

            Post post = _postService.FirstOrDefaultActive(x => x.Id == model.Id);

            if (post != null)
            {
                if (uploadImages != null && uploadImages.Count > 0)
                {
                    if (uploadImages.ToList().Count == 1)
                    {
                        ImageNumber = 1;
                    }
                    else
                    {
                        ImageNumber = 2;
                    }
                    _postImageService.saveImage(post.Id, uploadImages);
                }
                if (!String.IsNullOrEmpty(model.PostContent))
                {
                    hasText = true;
                }

                if (deleteImages != null && deleteImages.Count > 0)
                {
                    foreach (var delete in deleteImages)
                    {
                        PostImage img = _postImageService.FirstOrDefaultActive(x => x.Id == delete);
                        _postImageService.Delete(img);
                    }
                }

                post.PostContent = model.PostContent;
                if (ImageNumber == 0 && hasText)
                {
                    post.ContentType = (int)ContentPostType.TextOnly;
                }
                else if (ImageNumber == 1 && hasText)
                {
                    post.ContentType = (int)ContentPostType.TextAndImage;
                }
                else if (ImageNumber == 2 && hasText)
                {
                    post.ContentType = (int)ContentPostType.TextAndMultiImages;
                }
                else if (ImageNumber == 1 && !hasText)
                {
                    post.ContentType = (int)ContentPostType.ImageOnly;
                }
                else if (ImageNumber == 2 && !hasText)
                {
                    post.ContentType = (int)ContentPostType.MultiImages;
                }

                post.EditDate = DateTime.Now;
                post.LatestInteractionTime = post.EditDate;
                _postService.Update(post);
                _postService.Save();

                if (!String.IsNullOrEmpty(sportSelect))
                {
                    string[] sportId = sportSelect.Split(',');
                    if (sportId != null)
                    {
                        var _postSport = this.Service<IPostSportService>();
                        IEnumerable<PostSport> sportList = _postSport.GetActive(x => x.PostId == post.Id);
                        foreach (var sport in sportList)
                        {
                            _postSport.Delete(sport);
                        }

                        foreach (var item in sportId)
                        {
                            if (!item.Equals(""))
                            {
                                PostSport postSport = new PostSport();
                                postSport.PostId = post.Id;
                                postSport.SportId = Int32.Parse(item);
                                _postSport.Create(postSport);
                            }
                        }
                    }
                }
            }
            return Json(result);
        }

        private NotificationCustomViewModel PrepareNotificationCustomViewModel(Notification noti)
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
    }
}