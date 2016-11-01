using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.ViewModels;
using SportsSocialNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
    public class EventController : BaseController
    {
        // GET: Event
        public ActionResult Index()
        {
            var _sportService = this.Service<ISportService>();
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            var sportList = _sportService.getAllSport();
            IEnumerable<SelectListItem> selectList = sportList.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToArray();
            ViewBag.SportList = selectList;

            var province = vietnam.VietNamese.ToList();
            IEnumerable<SelectListItem> provinceList = province.Select(m => new SelectListItem
            {
                Text = m.Type + " " + m.Name,
                Value = m.Name
            }).OrderBy(s => s.Value).ToArray();
            ViewBag.ProvinceList = provinceList;
            var viewModel = new SearchPlaceViewModel();
            return View(viewModel);
        }

        public ActionResult PlaceEvent(int? id)
        {
            var _placeService = this.Service<IPlaceService>();
            var place = _placeService.FirstOrDefaultActive(p => p.Id == id);
            if(place == null)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }
            ViewBag.placeName = place.Name;
            ViewBag.placeId = id;
            var viewModel = new EventViewModel();

            return View(viewModel);
        }

        public ActionResult GetDistrict(string provinceName)
        {
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            IEnumerable<SelectListItem> districtList = new List<SelectListItem>();
            if (provinceName != null || provinceName != "")
            {
                var province = vietnam.VietNamese.Where(p => p.Name.Equals(provinceName)).ToList();
                if (province != null && province.Count > 0)
                {
                    var district = province.FirstOrDefault().Districts.ToList();
                    districtList = district.Select(m => new SelectListItem
                    {
                        Text = m.Type + " " + m.Name,
                        Value = m.Name
                    }).OrderBy(s => s.Value).ToArray();
                }

            }

            return Json(districtList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData(JQueryDataTableParamModel param, string placeId, string province,
            string district, string lat, string lng)

        {

            var _placeService = this.Service<IPlaceService>();
            var _eventService = this.Service<IEventService>();
            List<Event> eventList = new List<Event>();
            if (placeId!=null)
            {
                int placeID = Int32.Parse(placeId);
                var placeEvents = _eventService.GetActive(p => p.PlaceId == placeID).OrderByDescending(p =>
                           p.EndDate).ToList();
                foreach (Event placeEvent in placeEvents)
                {
                    eventList.Add(placeEvent);
                }
            }
            else
            {
               
                if (lat != null && lat != "" && lng != null && lng != "")
                {
                    var places = _placeService.getAllPlace();
                    var latitude = float.Parse(lat);
                    var longtitude = float.Parse(lng);
                    var Coord = new GeoCoordinate(latitude, longtitude);
                    foreach (Place place in places)
                    {
                        var placeCoord = new GeoCoordinate(place.Latitude.Value, place.Longitude.Value);
                        var dis = Coord.GetDistanceTo(placeCoord);
                        if (Coord.GetDistanceTo(placeCoord) < 5000)
                        {
                            var placeEvents = _eventService.GetActive(p => p.PlaceId == place.Id).OrderByDescending(p =>
                            p.EndDate).ToList();
                            foreach (Event placeEvent in placeEvents)
                            {
                                eventList.Add(placeEvent);
                            }
                        }
                    }
                }
                else
                {
                    var places = _placeService.getPlace(null, province, district).ToList();
                    foreach (Place place in places)
                    {
                        var placeEvents = _eventService.GetActive(p => p.PlaceId == place.Id).OrderByDescending(p =>
                           p.EndDate).ToList();
                        foreach (Event placeEvent in placeEvents)
                        {
                            eventList.Add(placeEvent);
                        }
                    }
                }
            }

            IEnumerable<Event> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = eventList.Where(
                    d => (d.Name != null && d.Name.ToLower().Contains(param.sSearch.ToLower()))
                    || (d.Place.Name != null && d.Place.Name.ToLower().Contains(param.sSearch.ToLower()))
                ).OrderByDescending(d =>
                       d.EndDate);
            }
            else
            {
                filteredListItems = eventList.OrderByDescending(p =>
                       p.EndDate);
            }
            // Sort.
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc

            switch (sortColumnIndex)
            {
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(c => c.EndDate)
                        : filteredListItems.OrderByDescending(c => c.EndDate);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(c => new IConvertible[]{
                c.Id,
                (c.Image == null || c.Image == "")?"/Content/images/no_image.jpg":c.Image,
                c.Name,
                c.StartDate.ToString("dd/MM/yyyy")+" - "+c.EndDate.ToString("dd/MM/yyyy"),
                c.Place.Id,
                c.Place.Name,
                Utils.GetEnumDescription((EventStatus)c.Status)
                //c.Fields.Count == 0? "": c.Fields.Select(d => d.FieldType.Sport).ToString()
            }.ToArray());

            return Json(new
            {
                param.sEcho,
                iTotalRecords = result.Count(),
                iTotalDisplayRecords = filteredListItems.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewDetail(int? id)
        {
            var _eventService = this.Service<IEventService>();
            var _placeService = this.Service<IPlaceService>();
            var entity = _eventService.FirstOrDefaultActive(p => p.Id == id);
            var events = new EventViewModel(entity);
            ViewBag.EventStatus = Utils.GetEnumDescription((EventStatus)events.Status);
            ViewBag.Place = _placeService.FirstOrDefaultActive(p => p.Id == events.PlaceId);
            return View(events);
        }
    }
}