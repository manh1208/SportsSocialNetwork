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
using SportsSocialNetwork.Models.Identity;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.Enumerable;
using Microsoft.AspNet.Identity;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
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
            var viewModel = new SearchPlaceViewModel();
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
            List<PlaceOveralViewModel> resultList = new List<PlaceOveralViewModel>();

            if (lat != null && lat != "" && lng != null && lng != "")
            {
                var places = _placeService.getAllPlace();
                if (sport != null && sport != "")
                {
                    int sportID = Int32.Parse(sport);
                    places = _placeService.GetActive(p => (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing) && 
                    p.Fields.Where(f => f.FieldType.SportId == sportID).ToList().Count > 0);
                }
                var latitude = float.Parse(lat);
                var longtitude = float.Parse(lng);
                var Coord = new GeoCoordinate(latitude, longtitude);
                foreach (Place place in places)
                {
                    var placeCoord = new GeoCoordinate(place.Latitude.Value, place.Longitude.Value);
                    var dis = Coord.GetDistanceTo(placeCoord);
                    if (Coord.GetDistanceTo(placeCoord) < 5000)
                    {
                        placeList.Add(place);
                    }
                }
            }
            else
            {
                var tmp = province;
                Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
                IEnumerable<SelectListItem> districtList = new List<SelectListItem>();
                if (province != null || province != "")
                {

                    var provinceName = vietnam.VietNamese.Where(p => p.Name.Equals(province)).ToList();
                    if (province != null && provinceName.Count > 0)
                    {
                        var p = provinceName.First().Type;
                        province = p+" "+province;
                    }

                }

                IEnumerable<SelectListItem> wardList = new List<SelectListItem>();
                if (district != null || district != "")
                {
                    var provinceList = vietnam.VietNamese.Where(p => p.Name.Equals(tmp) && p.Districts.Where(f =>
                    f.Name == district).ToList().Count > 0).ToList();
                    if (provinceList != null && provinceList.Count > 0)
                    {
                        var districts = provinceList.First().Districts.Where(p => p.Name == district).ToList();
                        if (districts != null && districts.Count > 0)
                        {
                            var d = districts.First().Type;
                            district = d + " " + district;
                        }

                    }
                }
                    placeList = _placeService.getPlace(sport, province, district).ToList();
            }
            
            if(placeList!=null && placeList.Count > 0)
            {
                var rateService = this.Service<IRatingService>();
                foreach(var item in placeList)
                {
                    PlaceOveralViewModel model = Mapper.Map<PlaceOveralViewModel>(item);
                    var rates = rateService.GetActive(p => p.PlaceId == item.Id).ToList();
                    double averagePoint = 0;
                    if(rates!=null && rates.Count > 0)
                    {
                        foreach(var itemRate in rates)
                        {
                            averagePoint += itemRate.Point;
                        }
                        averagePoint = averagePoint / rates.Count;
                    }
                    model.rate = averagePoint;
                    resultList.Add(model);
                }
            }

            IEnumerable<PlaceOveralViewModel> filteredListItems;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredListItems = resultList.Where(
                    d => (d.Name != null && d.Name.ToLower().Contains(param.sSearch.ToLower()))
                ).OrderByDescending(p => p.rate);
            }
            else
            {
                filteredListItems = resultList.OrderByDescending(p => p.rate);
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
                (c.Avatar == null || c.Avatar.Equals(""))?"/Content/images/no_image.jpg":c.Avatar,
                c.Name,
                c.Description.Length > 140? c.Description.Substring(0,140)+"...": c.Description,
                (c.Address +", "+c.District+", "+c.City).Length < 60? (c.Address +", "+c.District+", "+c.City):
                (c.Address +", "+c.District+", "+c.City).Substring(0, 60)+"...",
                c.PhoneNumber
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
            var entity = _placeService.FirstOrDefaultActive(p => p.Id == id.Value && 
            (p.Status == (int)PlaceStatus.Active || p.Status == (int)PlaceStatus.Repairing));
            if(entity == null)
            {
                return RedirectToAction("PageNotFound", "Errors");
            }
            var place = new PlaceViewModel(entity);
            var _placeImageService = this.Service<IPlaceImageService>();
            var _placeFieldService = this.Service<IFieldService>();
            var _placeEventService = this.Service<IEventService>();
            var _fieldTypeService = this.Service<IFieldTypeService>();
            var _sportService = this.Service<ISportService>();
            List<PlaceImage> placeImages = _placeImageService.GetActive(p => p.PlaceId == id.Value).ToList();
            List<Field> placeFields = _placeFieldService.GetActive(p => p.PlaceId == id.Value).ToList();
            List<Event> placeEvents = _placeEventService.GetActive(p => p.PlaceId == id.Value).ToList();
            List<FieldType> fieldTypes = _fieldTypeService.GetActive().ToList();
            Event lastestEvent = new Event();
            if (placeEvents != null && placeEvents.Count > 0)
            {
                lastestEvent = placeEvents.First();
                ViewBag.lastestEvent = lastestEvent;
            }
            ViewBag.placeImages = placeImages;
            ViewBag.placeFields = placeFields;
            ViewBag.PlaceStatus = Utils.GetEnumDescription((PlaceStatus)place.Status);

            var list = _placeFieldService.GetActive(p => p.PlaceId == id.Value).
        Join(_fieldTypeService.GetActive(),
        f => f.FieldTypeId, ft => ft.Id,
        (f, ft) => new
        {
            fieldID = f.Id,
            fieldName = f.Name,
            sportId = ft.SportId
        }).Join(_sportService.GetActive(),
                a => a.sportId, p => p.Id,
                (a, p) => new
                {
                    fieldID = a.fieldID,
                    fieldName = a.fieldName,
                    sport = p.Name
                }).Select(p => new FieldSportViewModel
                {
                    FieldId = p.fieldID,
                    FieldName = p.fieldName,
                    Sport = p.sport
                }).ToList();
            ViewBag.fieldSport = list;
            return View(place);
        }

        public ActionResult GetRateInfo(int id)
        {
            var result = new AjaxOperationResult<RateInfoViewModel>();
            var rateService = this.Service<IRatingService>();
            var rate = rateService.GetActive(p => p.PlaceId == id).ToList();
            RateInfoViewModel model = new RateInfoViewModel();
            int numOfRate = 0;
            double averagePoint = 0;
            if (rate != null && rate.Count > 0)
            {
                foreach(var item in rate)
                {
                    averagePoint += item.Point;
                }
                numOfRate = rate.Count;
                averagePoint = averagePoint / numOfRate;
            }
            model.AverageRate = Math.Round(averagePoint,1);
            model.NumberOfRate = numOfRate;
            result.Succeed = true;
            result.AdditionalData = model;
            return Json(result);
        }

        public ActionResult LoadRating(int placeId)
        {
            var result = new AjaxOperationResult<RateInfoViewModel>();
            var rateService = this.Service<IRatingService>();
            var userId = User.Identity.GetUserId();
            var rate = rateService.FirstOrDefault(p => p.PlaceId == placeId && p.UserId == userId);
            RateInfoViewModel model = new RateInfoViewModel();
            if (rate != null)
            {
                model.AverageRate = rate.Point;
            }else
            {
                model.AverageRate = 0;
            }
            result.Succeed = true;
            result.AdditionalData = model;
            return Json(result);
        }

        public ActionResult Rating(int placeId, int score)
        {
            var result = new AjaxOperationResult();
            var rateService = this.Service<IRatingService>();
            var userId = User.Identity.GetUserId();
            var rate = rateService.FirstOrDefaultActive(p => p.PlaceId == placeId && p.UserId == userId);
            if (rate != null)
            {
                rate.Point = score;
                rateService.Update(rate);
                rateService.Save();
            }
            else
            {
                Rating rating = new Rating();
                rating.PlaceId = placeId;
                rating.Point = score;
                rating.UserId = userId;
                rateService.Create(rating);
                rateService.Save();
            }
            result.Succeed = true;
            return Json(result);
        }
    }

}