using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SportsSocialNetwork.Models.Entities;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using SportsSocialNetwork.Models.Enumerable;

namespace SportsSocialNetwork.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Index()
        {
            var _newsService = this.Service<INewsService>();
            var _categoryService = this.Service<ICategoryService>();

            List<Category> categories = _categoryService.GetActive().ToList();
            List<News> PopularNews = _newsService.GetPopularNews();
            Dictionary<Category, List<News>> ListNews = _newsService.GetNewsDependOnHobbies(categories);

            ViewBag.categories = categories;
            ViewBag.PopularNews = PopularNews;
            ViewBag.ListNews = ListNews;
            return View();
        }

        public ActionResult NewsDetail(int? id)
        {
            var _newsService = this.Service<INewsService>();
            var _categoryService = this.Service<ICategoryService>();
            var _followService = this.Service<IFollowService>();
            var _userService = this.Service<IAspNetUserService>();
            var _sportService = this.Service<ISportService>();

            string curUserId = User.Identity.GetUserId();

            _newsService.UpdateNumOfRead(id.Value);
            List<Category> categories = _categoryService.GetActive().ToList();
            News news = _newsService.GetNewsById(id.Value);
            NewsViewModel newsVM = Mapper.Map<NewsViewModel>(news);
            List<News> relativeNews = _newsService.GetRelativeNews(id.Value);


            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;

            //get list of user that this user is following
            List<Follow> followingList = _followService.GetActive(f => f.FollowerId == curUserId).ToList();
            List<FollowDetailViewModel> followingListVM = Mapper.Map<List<FollowDetailViewModel>>(followingList);
            foreach (var item in followingListVM)
            {
                AspNetUser user = _userService.FirstOrDefaultActive(u => u.Id.Equals(item.UserId));
                AspNetUserViewModel userVM = Mapper.Map<AspNetUserViewModel>(user);
                item.User = userVM;
            }
            ViewBag.followingList = followingListVM;

            //load group name
            var _groupService = this.Service<IGroupService>();
            List<Group> groupList = _groupService.GetActive(p => p.GroupMembers.Where(f =>
           f.UserId == curUserId && f.Status == (int)GroupMemberStatus.Approved).ToList().Count > 0).ToList();
            if (groupList != null)
            {
                ViewBag.GroupList = groupList;
            }

            ViewBag.categories = categories;
            ViewBag.relativeNews = relativeNews;

            return View(newsVM);
        }

        public ActionResult Category(int? id)
        {
            var _newsService = this.Service<INewsService>();
            var _categoryService = this.Service<ICategoryService>();

            List<Category> categories = _categoryService.GetActive().ToList();
            List<News> popularNewByCate = _newsService.GetPopularNewsByCategory(id.Value);
            List<News> newsByCate = _newsService.GetNewsByCategory(id.Value);

            ViewBag.categories = categories;
            ViewBag.popularNewByCate = popularNewByCate;
            ViewBag.newsByCate = newsByCate;
            return View();
        }

        [HttpPost]
        public ActionResult LoadNewsComments(int id, int skip, int take)
        {
            var result = new AjaxOperationResult<List<NewsCommentDetailViewModel>>();
            if (id != 0)
            {
                var _newsCommentService = this.Service<INewsCommentService>();
                var _postService = this.Service<IPostService>();
                

                List<NewsComment> newsComtList = _newsCommentService.GetNewsComments(id, skip, take).ToList();
                List<NewsCommentDetailViewModel> newsCmtListVM = Mapper.Map<List<NewsCommentDetailViewModel>>(newsComtList);
                foreach (var item in newsCmtListVM)
                {
                    item.CommentAge = _postService.CalculatePostAge(item.CreateDate);
                }

                result.AdditionalData = newsCmtListVM;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult CreateNewsComment(string userId, int newsId, string comment)
        {
            var _newsCommentService = this.Service<INewsCommentService>();
            var result = new AjaxOperationResult<NewsComment>();

            if(!String.IsNullOrEmpty(userId) && newsId != 0 && !String.IsNullOrEmpty(comment))
            {
                NewsComment nc = new NewsComment();
                nc.UserId = userId;
                nc.NewsId = newsId;
                nc.Comment = comment;
                nc.CreateDate = DateTime.Now;
                _newsCommentService.Create(nc);
                _newsCommentService.Save();

                result.AdditionalData = nc;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetNewsComtById(int id)
        {
            var _newsCommentService = this.Service<INewsCommentService>();
            var _postService = this.Service<IPostService>();
            var result = new AjaxOperationResult<NewsCommentDetailViewModel>();

            NewsComment nc = _newsCommentService.FirstOrDefaultActive(n => n.Id == id);
            if(nc != null)
            {
                NewsCommentDetailViewModel model = Mapper.Map<NewsCommentDetailViewModel>(nc);
                model.CommentAge = _postService.CalculatePostAge(model.CreateDate);
                result.AdditionalData = model;
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }

            return Json(result);
        }

        [HttpPost]
        public string GetAllNewsCmtCount(int id)
        {
            var _newsCommentService = this.Service<INewsCommentService>();
            int count = 0;
            List<NewsComment> newsComtList = _newsCommentService.GetActive(n => n.NewsId == id).ToList();
            if(newsComtList != null && newsComtList.Count > 0)
            {
                count = newsComtList.Count;
            }
            return count.ToString();
        }
    }
}