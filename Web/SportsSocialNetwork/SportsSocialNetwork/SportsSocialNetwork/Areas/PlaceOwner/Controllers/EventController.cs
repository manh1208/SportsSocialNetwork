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

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    [MyAuthorize(Roles = "Chủ sân")]
    public class EventController : Controller
    {
        // GET: PlaceOwner/Event
        public ActionResult Index()
        {
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

        [HttpPost]
        public ActionResult createEvent(Event evt, HttpPostedFileBase image)
        {
            var _eventService = this.Service<IEventService>();
            _eventService.saveEvent(evt, image);

            return RedirectToAction("index");
        }

        public ActionResult updateEvent(Event evt, HttpPostedFileBase image)
        {
            var result = new AjaxOperationResult();
            try
            {
                var _eventService = this.Service<IEventService>();
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
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(e => e.Name)
                        : filteredListItems.OrderByDescending(e => e.Name);
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