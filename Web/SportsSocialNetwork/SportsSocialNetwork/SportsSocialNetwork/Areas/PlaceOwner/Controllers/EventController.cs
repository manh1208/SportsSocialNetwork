using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    public class EventController : Controller
    {
        // GET: PlaceOwner/Event
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        public ActionResult EventDetail(int id)
        {
            var _eventService = this.Service<IEventService>();
            Event evt = _eventService.FirstOrDefault(e => e.Id == id);

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
            //var blogPostList = _blogPostService.GetBlogPostbyStoreId();
            var _eventService = this.Service<IEventService>();
            var eventList = _eventService.GetActive();
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