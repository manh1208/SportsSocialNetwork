using HenchmenWeb.Models.Notifications;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Hubs;
using SportsSocialNetwork.Models.Notifications;
using SportsSocialNetwork.Models.Utilities;
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
            post.ProfileId = User.Identity.GetUserId();


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
                string[] tmp = sportId.Distinct().ToArray();
                if (sportId != null)
                {
                    var _postSport = this.Service<IPostSportService>();
                    var postSport = new PostSport();
                    foreach (var item in tmp)
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

            //send noti when others post to your profile
            if(userId != profileId)
            {
                var _notificationService = this.Service<INotificationService>();
                var _userService = this.Service<IAspNetUserService>();

                AspNetUser sender = _userService.FindUser(userId);

                string title = Utils.GetEnumDescription(NotificationType.Post);
                int type = (int)NotificationType.Post;
                string message = sender.FullName + " đã đăng một bài viết lên tường nhà bạn";

                Notification noti = _notificationService.CreateNoti(profileId, userId, title, message, type, post.Id, null, null, null);

                //////////////////////////////////////////////
                //signalR noti
                NotificationFullInfoViewModel notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                // Get the context for the Pusher hub
                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                // Notify clients in the group
                hubContext.Clients.User(notiModel.UserId).send(notiModel);
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
            var _groupService = this.Service<IGroupService>();

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
                        Group g = _groupService.FindGroupById(groupId);
                        Notification noti = _notiService.CreateNoti(member.UserId, post.UserId, Utils.GetEnumDescription(NotificationType.GroupPost), postedUser.FullName + " đã đăng một bài viết trong nhóm " +  g.Name, (int)NotificationType.GroupPost, post.Id, null, null, groupId);

                        List<string> registrationIds = GetToken(member.UserId);

                        NotificationModel notiModel = Mapper.Map<NotificationModel>(PrepareNotificationCustomViewModel(noti));

                        if (registrationIds != null && registrationIds.Count != 0)
                        {
                            Android.Notify(registrationIds, null, notiModel);
                        }

                        //signalR noti
                        NotificationFullInfoViewModel notiModelR = _notiService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                        // Get the context for the Pusher hub
                        IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();

                        // Notify clients in the group
                        hubContext.Clients.User(notiModel.UserId).send(notiModelR);
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
                    string[] tmp = sportId.Distinct().ToArray();
                    if (sportId != null)
                    {
                        
                        foreach (var item in tmp)
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

            var _userService = this.Service<IAspNetUserService>();
            AspNetUser us = _userService.FirstOrDefaultActive(u => u.Id.Equals(noti.FromUserId));
            if (us != null)
            {
                result.Avatar = us.AvatarImage;
            }
            else
            {
                result.Avatar = "";
            }

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

        [HttpPost]
        public ActionResult CreateSharePost(string userId, int shareType, int dataId, int receiver, string frdSelectShare, Nullable<int> groupSelectShare, string shareContent, string sportSelectShare)
        {
            var result = new AjaxOperationResult();
            var _postService = this.Service<IPostService>();
            var _userService = this.Service<IAspNetUserService>();
            var _groupMemberService = this.Service<IGroupMemberService>();
            var _groupService = this.Service<IGroupService>();

            int notiType = -1;

            Post post = new Post();

            post.UserId = userId;
            post.PostContent = shareContent;

            string notiMessType = "";

            if (shareType != 0)
            {
                switch (shareType)
                {
                    case (int)ContentPostType.SharePostPost:
                        Post oldPost = _postService.FirstOrDefaultActive(p => p.Id == dataId);
                        if(oldPost != null)
                        {
                            if(oldPost.PostId != null || oldPost.OrderId != null || oldPost.EventId != null || oldPost.NewsId != null)
                            {
                                post = oldPost;
                                post.UserId = userId;
                                post.PostContent = shareContent;
                            }
                            else
                            {
                                post.ContentType = (int)ContentPostType.SharePostPost;
                                post.PostId = dataId;
                            }
                        }

                        notiMessType = "bài viết";
                        break;
                    case (int)ContentPostType.ShareEventPost:
                        post.ContentType = (int)ContentPostType.ShareEventPost;
                        post.EventId = dataId;

                        notiMessType = "sự kiện";

                        break;
                    case (int)ContentPostType.ShareOrderPost:
                        post.ContentType = (int)ContentPostType.ShareOrderPost;
                        post.OrderId = dataId;

                        notiMessType = "lịch hoạt động";

                        break;
                    case (int)ContentPostType.ShareNewsPost:
                        post.ContentType = (int)ContentPostType.ShareNewsPost;
                        post.NewsId = dataId;

                        notiMessType = "tin tức";

                        break;
                }

                if (receiver != 0)
                {
                    switch (receiver)
                    {
                        case (int)SharedReceiver.SenderWall:
                            post.ProfileId = userId;

                            break;
                        case (int)SharedReceiver.FriendWall:
                            post.ProfileId = frdSelectShare;
                            notiType = (int)NotificationType.ShareFrdWall;

                            break;
                        case (int)SharedReceiver.Group:
                            post.GroupId = groupSelectShare;
                            notiType = (int)NotificationType.ShareGroup;

                            break;
                    }

                    if (_postService.CreatePost(post) != null)
                    {
                        if (!String.IsNullOrEmpty(sportSelectShare))
                        {
                            string[] sportId = sportSelectShare.Split(',');
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
                        result.Succeed = true;
                    }
                    else
                    {
                        result.Succeed = false;
                    }
                }
                else
                {
                    result.Succeed = false;
                }
            }
            else
            {
                result.Succeed = false;
            }

            //=============NOTI===================================================================================
            //save noti
            AspNetUser sender = _userService.FindUser(userId);
            var _notificationService = this.Service<INotificationService>();

            string title = Utils.GetEnumDescription(NotificationType.Post);
            int type = (int)NotificationType.Post;
            string message = "";
            Notification noti;

            // Get the context for the Pusher hub
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RealTimeHub>();
            NotificationFullInfoViewModel notiModel;

            if (sender != null)
            {
                if (receiver != 0)
                {
                    switch (receiver)
                    {
                        case (int)SharedReceiver.FriendWall:

                            //for noti
                            AspNetUser noti_receiver = _userService.FindUser(frdSelectShare);
                            message = sender.FullName + " đã chia sẻ một " + notiMessType + " lên tường nhà bạn";
                            noti = _notificationService.CreateNoti(noti_receiver.Id, sender.Id, title, message, notiType, null, null, null, null);

                            //signalR noti
                            notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                            // Notify clients in the group
                            hubContext.Clients.User(notiModel.UserId).send(notiModel);

                            break;
                        case (int)SharedReceiver.Group:

                            //for noti
                            Group noti_group = _groupService.FindGroupById(groupSelectShare.Value);
                            List<GroupMember> noti_gm = _groupMemberService.GetActive(g => g.GroupId == groupSelectShare.Value && g.Status == (int)GroupMemberStatus.Approved && (!(g.UserId.Equals(userId)))).ToList();

                            message = sender.FullName + " đã chia sẻ một " + notiMessType + " trong nhóm " + noti_group.Name;
                            foreach (var item in noti_gm)
                            {
                                noti = _notificationService.CreateNoti(item.UserId, sender.Id, title, message, notiType, null, null, null, noti_group.Id);

                                //signalR noti
                                notiModel = _notificationService.PrepareNoti(Mapper.Map<NotificationFullInfoViewModel>(noti));

                                // Notify clients in the group
                                hubContext.Clients.User(notiModel.UserId).send(notiModel);
                            }

                            break;
                    }
                }
            }

            return Json(result);
        }
    }
}