using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using Microsoft.AspNet.Identity;
using SportsSocialNetwork.Models.Identity;
using System.Globalization;
using SkyWeb.DatVM.Mvc;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    [MyAuthorize(Roles = "Chủ sân")]
    public class EventController : BaseController
    {
        // GET: PlaceOwner/Event
        public ActionResult Index()
        {
            var _sportService = this.Service<ISportService>();
            var _followService = this.Service<IFollowService>();
            var _userService = this.Service<IAspNetUserService>();

            string curUserId = User.Identity.GetUserId();
            //get sport list for post
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

            return View();
        }

        public ActionResult CreateEvent()
        {
            var _placeService = this.Service<IPlaceService>();
            string curUserID = User.Identity.GetUserId();
            List<Place> places = _placeService.GetActive(p => p.UserId.Equals(curUserID)).ToList();
            List<SelectListItem> sPlaces = new List<SelectListItem>();
            if (places != null && places.Count > 0)
            {
                foreach (var item in places)
                {
                    sPlaces.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            else
            {
                sPlaces.Add(new SelectListItem { Text = "Hiện chưa có địa điểm", Value = "" });
            }

            ViewBag.places = sPlaces;
            return View();
        }

        public ActionResult EventDetail(int id)
        {
            var _eventService = this.Service<IEventService>();
            Event evt = _eventService.FirstOrDefault(e => e.Id == id);
            List<SelectListItem> statuss = new List<SelectListItem>();

            foreach (EventStatus item in Enum.GetValues(typeof(EventStatus)))
            {
                statuss.Add(new SelectListItem { Value = Convert.ToString((int)item), Text = Utils.GetEnumDescription(item)});
            }

            var _placeService = this.Service<IPlaceService>();
            string userID = User.Identity.GetUserId();
            List<Place> places = _placeService.GetActive(p => p.UserId.Contains(userID)).ToList();
            List<SelectListItem> sPlaces = new List<SelectListItem>();
            if (places != null && places.Count > 0)
            {
                foreach (var item in places)
                {
                    sPlaces.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }
            }
            else
            {
                sPlaces.Add(new SelectListItem { Text = "Hiện chưa có địa điểm", Value = "" });
            }

            ViewBag.statusList = statuss;
            ViewBag.places = sPlaces;
            return this.PartialView(evt);
        }

        public ActionResult EventDetailModal(int id)
        {
            var _eventService = this.Service<IEventService>();
            Event evt = _eventService.FirstOrDefault(e => e.Id == id);

            return this.PartialView(evt);
        }

        public ActionResult ShareEventModal(int id)
        {
            var _eventService = this.Service<IEventService>();
            var _sportService = this.Service<ISportService>();
            Event evt = _eventService.FirstOrDefault(e => e.Id == id);

            //get sport list for post
            var sports = _sportService.GetActive()
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString()
                            }).OrderBy(s => s.Value);
            ViewBag.Sport = sports;

            return this.PartialView(evt);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createEvent(Event evt, HttpPostedFileBase image)
        {
            var _eventService = this.Service<IEventService>();
            string stDate = Request["StartDate"];
            evt.StartDate = DateTime.ParseExact(stDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string enDate = Request["EndDate"];
            evt.EndDate = DateTime.ParseExact(enDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            _eventService.saveEvent(evt, image);

            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateEvent(Event evt, HttpPostedFileBase image)
        {
            var result = new AjaxOperationResult();
            try
            {
                var _eventService = this.Service<IEventService>();
                string stDate = Request["StartDate"];
                evt.StartDate = DateTime.ParseExact(stDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string enDate = Request["EndDate"];
                evt.EndDate = DateTime.ParseExact(enDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //string des = Request["Description"];
                //evt.Description = Request["Description"];
                _eventService.saveEvent(evt, image);
                result.Succeed = true;
            }
            catch(Exception e)
            {
                result.Succeed = false;
            }
            

            return Json(result);
        }

        public string deleteEvent(int id)
        {
            var _eventService = this.Service<IEventService>();
            Event evt = _eventService.FirstOrDefaultActive(e => e.Id == id);
            if (evt != null)
            {
                _eventService.Deactivate(evt);
                return "success";
            }
            return "false";
        }

        [HttpPost]
        public ActionResult shareEvent(int eventId, string postContent, string eventSport)
        {
            var _postService = this.Service<IPostService>();
            var _eventService = this.Service<IEventService>();
            var result = new AjaxOperationResult();
            bool hasImage = false;
            bool hasText = false;

            Event evt = _eventService.FirstOrDefaultActive(e => e.Id == eventId);

            if(evt != null)
            {
                //string pattern = "<.*?>";
                //string replacement = "";
                //Regex rgx = new Regex(pattern);
                //string rawContent = rgx.Replace(evt.Description, replacement);
                //string content = rawContent.Substring(0, Math.Min(rawContent.Length, 200));
                if(!String.IsNullOrEmpty(postContent))
                {
                    hasText = true;
                }
                if (!String.IsNullOrEmpty(evt.Image))
                {
                    hasImage = true;
                }

                Post post = new Post();
                if (hasText == true && hasImage == true)
                {
                    post.ContentType = (int)ContentPostType.TextAndImage;
                    post.PostContent = postContent;
                }
                else if(hasText == true && hasImage == false)
                {
                    post.ContentType = (int)ContentPostType.TextOnly;
                    post.PostContent = postContent;
                }
                else if(hasText == false && hasImage == true)
                {
                    post.ContentType = (int)ContentPostType.ImageOnly;
                }

                post.UserId = User.Identity.GetUserId();
                post.ProfileId = User.Identity.GetUserId();

                if(_postService.CreatePost(post) != null)
                {
                    //save images
                    if(hasImage)
                    {
                        var _postImageService = this.Service<IPostImageService>();
                        PostImage pi = new PostImage();
                        pi.PostId = post.Id;
                        pi.Image = evt.Image;
                        _postImageService.Create(pi);
                        _postImageService.Save();
                    }

                    //save hastag
                    if (!String.IsNullOrEmpty(eventSport))
                    {
                        string[] sportId = eventSport.Split(',');
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
            return Json(result);
        }

        public ActionResult GetData(JQueryDataTableParamModel param)
        {
            string userID = Request["userID"];
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _eventService = this.Service<IEventService>();

            var eventList = _eventService.GetActive(e => e.CreatorId == userID).ToList();

            
            //IEnumerable<BlogPost> filteredListItems;
            IEnumerable<Event> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = eventList.Where(p => p.Name != null && p.Name.ToLower().Contains(param.sSearch.ToLower()));

                //filteredListItems = blogPostList.Where(
                //    d => (d.Title != null && d.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.BlogCategory.Title != null && d.BlogCategory.Title.ToLower().Contains(param.sSearch.ToLower()))
                //    || (d.MetaDescription != null && d.MetaDescription.ToLower().Contains(param.sSearch.ToLower()))
                //);
            }
            else
            {
                //filteredListItems = blogPostList;
                filteredListItems = eventList;
            }
            // Sort.
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc

            switch (sortColumnIndex)
            {
                case 0:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(e => e.Name)
                        : filteredListItems.OrderByDescending(e => e.Name);
                    break;
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(e => e.StartDate)
                        : filteredListItems.OrderByDescending(e => e.StartDate);
                    break;
                case 3:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(e => e.EndDate)
                        : filteredListItems.OrderByDescending(e => e.EndDate);
                    break;
                case 4:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(e => e.Status)
                        : filteredListItems.OrderByDescending(e => e.Status);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength); var count = 1;
            var result = displayedList.Select(e => new IConvertible[]{
                //c.Id,
                //c.Image,
                //c.Title,
                //c.BlogCategoryId,
                //c.BlogCategory.Title,
                //c.Author,
                //c.Active,
                //c.Id

                e.Id,
                e.Name,
                e.CreatorId,
                e.StartDate,
                e.EndDate,
                e.Image,
                e.Status,
                count++

            }.ToArray());

            return Json(new
            {
                param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = filteredListItems.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);

        }
    }
}