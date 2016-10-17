using Newtonsoft.Json;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.API.Controllers
{
    public class PlaceController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        public PlaceController(IPlaceService placeService)
        {

        }

        [System.Web.Mvc.HttpGet]
        public ActionResult ShowAllPlaces(int skip, int take)
        {
            ResponseModel<List<PlaceOveralViewModel>> response = null;

            var service = this.Service<IPlaceService>();

            List<Place> placeList = null;

            try
            {
                placeList = service.GetAll(skip, take).ToList();
                List<PlaceOveralViewModel> result = Mapper.Map<List<PlaceOveralViewModel>>(placeList);

                foreach (var place in placeList)
                {
                    foreach (var r in result)
                    {
                        if (r.Id == place.Id)
                        {
                            r.SportList = GetAllSportOfPlace(place);
                        }

                    }
                }

                response = new ResponseModel<List<PlaceOveralViewModel>>(true, "Danh sách địa điểm đã tải thành công!", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<PlaceOveralViewModel>>.CreateErrorResponse("Danh sách địa điểm đã tải thất bại!", systemError);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ShowPlaceDetail(int id)
        {
            ResponseModel<PlaceDetailViewModel> response = null;
            var service = this.Service<IPlaceService>();

            try
            {
                Place place = service.GetPlaceById(id);
                if (place != null)
                {
                    PlaceDetailViewModel result = Mapper.Map<PlaceDetailViewModel>(place);

                    response = new ResponseModel<PlaceDetailViewModel>(true, "Thông tin địa điểm đã tải thành công", null, result);
                }
                else
                {
                    response = ResponseModel<PlaceDetailViewModel>.CreateErrorResponse("Thất bại khi tải thông tin địa điểm!", systemError);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<PlaceDetailViewModel>.CreateErrorResponse("Thất bại khi tải thông tin địa điểm!", systemError);
            }
            return Json(response);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult ChangePlaceStatus(int id, int status)
        {
            var service = this.Service<IPlaceService>();

            ResponseModel<PlaceViewModel> response = null;
            try
            {
                Place place = service.ChangeStatus(id, status);
                if (place != null)
                {
                    PlaceViewModel result = Mapper.Map<PlaceViewModel>(place);
                    response = new ResponseModel<PlaceViewModel>(true, "Trạng thái đã được cập nhật!", null, result);
                }
                else
                {
                    response = ResponseModel<PlaceViewModel>.CreateErrorResponse("Cập nhật trạng thái thất bại!", systemError);
                }
            }
            catch (Exception e)
            {
                response = ResponseModel<PlaceViewModel>.CreateErrorResponse("Cập nhật trạng thái thất bại!", systemError);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult findSurroundingPlace(string sport, string province,
            string district, string lat, string lng)
        {
            var placeService = this.Service<IPlaceService>();

            List<Place> placeList = new List<Place>();

            ResponseModel<List<PlaceViewModel>> response = null;

            try
            {
                if (lat != null && lat != "" && lng != null && lng != "")
                {
                    var places = placeService.getAllPlace();
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
                    placeList = placeService.getPlace(sport, province, district).ToList();
                }

                List<PlaceViewModel> result = Mapper.Map<List<PlaceViewModel>>(placeList);

                response = new ResponseModel<List<PlaceViewModel>>(true, "Những địa điểm gần bạn:", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<PlaceViewModel>>.CreateErrorResponse("Không tìm thấy kết quả", systemError);
            }
            return Json(response);
        }

        private List<SportViewModel> GetAllSportOfPlace(Place place)
        {
            List<SportViewModel> sportList = new List<SportViewModel>();

            List<Field> fieldList = place.Fields.ToList();

            foreach (var field in fieldList)
            {
                SportViewModel sport = Mapper.Map<SportViewModel>(field.FieldType.Sport);
                if (sportList.Count == 0)
                {
                    sportList.Add(sport);
                }
                else
                {
                    for (int i = 0; i < sportList.Count; i++)
                    {
                        var s = sportList[i];
                        if (s.Id != sport.Id)
                        {
                            sportList.Add(sport);
                        }
                    }
                }
            }

            return sportList;
        }
    }
}
