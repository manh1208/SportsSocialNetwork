using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using SportsSocialNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Device.Location;

namespace SportsSocialNetwork.Controllers
{
    public class PlaceController : BaseController
    {
        // GET: Place
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
            var viewModel =new  SearchPlaceViewModel();
            return View(viewModel);
        }

        public ActionResult GetDistrict(string provinceName)
        {
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            IEnumerable<SelectListItem> districtList = new List<SelectListItem>();
            if (provinceName != null || provinceName!="")
            {
                var province = vietnam.VietNamese.Where(p => p.Name.Equals(provinceName)).ToList();
                if (province != null && province.Count>0)
                {
                    var district = province.First().Districts.ToList();
                    districtList = district.Select(m => new SelectListItem
                    {
                        Text = m.Type + " " + m.Name,
                        Value = m.Name
                    }).OrderBy(s => s.Value).ToArray();
                }
                
            }
           
            return Json(districtList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData(JQueryDataTableParamModel param, string sport, string province, 
            string district, string lat, string lng)
        {
            var _placeService = this.Service<IPlaceService>();
            List<Place> placeList = new List<Place>();
            

            if(lat!=null && lat!="" && lng!=null && lng != "")
            {
                var places = _placeService.getAllPlace();
                var latitude = float.Parse(lat);
                var longtitude = float.Parse(lng);
                var Coord = new GeoCoordinate(latitude, longtitude);
                foreach(Place place in places)
                {
                    var placeCoord = new GeoCoordinate(place.Latitude, place.Longitude);
                    var dis = Coord.GetDistanceTo(placeCoord);
                    if (Coord.GetDistanceTo(placeCoord) < 5000)
                    {
                        placeList.Add(place);
                    }
                }
            }
            else
            {
                placeList = _placeService.getPlace(sport, province, district).ToList();
            }
            
            IEnumerable<Place> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = placeList.Where(
                    d => (d.Name != null && d.Name.ToLower().Contains(param.sSearch.ToLower()))
                );
            }
            else
            {
                filteredListItems = placeList;
            }
            // Sort.
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc

            switch (sortColumnIndex)
            {
                case 2:
                    filteredListItems = sortDirection == "asc"
                        ? filteredListItems.OrderBy(c => c.Name)
                        : filteredListItems.OrderByDescending(c => c.Name);
                    break;
            }

            var displayedList = filteredListItems.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = displayedList.Select(c => new IConvertible[]{
                c.Id,
                c.PlaceImages.Count == 0?"http://raovatso.net/images/no-image.jpg":c.PlaceImages.First().Image,
                c.Name,
                c.Description,
                c.Address,
                c.District,
                c.City,
                c.PhoneNumber
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

        public ActionResult Test()
        {
            return View();
        }
        public ActionResult ViewDetail(int? id)
        {
            var _placeService = this.Service<IPlaceService>();
            var entity = _placeService.FirstOrDefaultActive(p => p.Id == id.Value);
            var place = new PlaceViewModel(entity);
            var _placeImageService = this.Service<IPlaceImageService>();
            var _placeFieldService = this.Service<IFieldService>();
            var _placeEventService = this.Service<IEventService>();
            List<PlaceImage> placeImages = _placeImageService.Get(p => p.PlaceId == id.Value).ToList();
            List<Field> placeFields = _placeFieldService.Get(p => p.PlaceId == id.Value).ToList();
            List<Event> placeEvents = _placeEventService.Get(p => p.PlaceId == id.Value).ToList();
            Event lastestEvent = new Event();
            if (placeEvents!=null && placeEvents.Count > 0)
            {
                lastestEvent = placeEvents.First();
            }
            ViewBag.placeImages = placeImages;
            ViewBag.placeFields = placeFields;
            ViewBag.lastestEvent = lastestEvent;
            return View(place);
        }
    }
    
}