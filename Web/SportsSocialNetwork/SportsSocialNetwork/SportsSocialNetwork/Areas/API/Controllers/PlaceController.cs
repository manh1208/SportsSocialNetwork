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
        private String systemError = "An error has occured!";

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
                placeList = service.GetAll(skip,take).ToList();
                List<PlaceOveralViewModel> result = Mapper.Map<List<PlaceOveralViewModel>>(placeList);

                foreach (var place in placeList)
                {
                    PlaceImage image = place.PlaceImages.FirstOrDefault();
                    if (image != null)
                    {
                        foreach (var detailPlace in result)
                        {
                            if (image.PlaceId == detailPlace.Id)
                            {
                                detailPlace.Avatar = image.Image;
                            }
                        }
                    }
                }

                response = new ResponseModel<List<PlaceOveralViewModel>>(true, "Load Place List Successfully", null, result);
            }
            catch (Exception)
            {
                response = ResponseModel<List<PlaceOveralViewModel>>.CreateErrorResponse("Place List failed to load!", systemError);
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

                    List<PlaceImage> images = place.PlaceImages.ToList<PlaceImage>();

                    List<String> imageList = new List<string>();

                    foreach (var image in images)
                    {
                        imageList.Add(image.Image);
                    }

                    result.imageList = imageList;

                    response = new ResponseModel<PlaceDetailViewModel>(true, "Place info Loaded", null, result);
                }
                else {
                    response = ResponseModel<PlaceDetailViewModel>.CreateErrorResponse("Place info failed to load!", systemError);
                }
            }
            catch (Exception)
            {
                response = ResponseModel<PlaceDetailViewModel>.CreateErrorResponse("Place info failed to load!", systemError);
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
                    response = new ResponseModel<PlaceViewModel>(true, "Your Place's status has been updated!", null, result);
                }
                else {
                    response = ResponseModel<PlaceViewModel>.CreateErrorResponse("Your Place's status has NOT been updated!", systemError);
                }
            }
            catch (Exception e)
            {
                response = ResponseModel<PlaceViewModel>.CreateErrorResponse("Your Place's status has NOT been updated!", systemError);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult findSurroundingPlace(string sport, string province,
            string district, string lat, string lng) {
            var placeService = this.Service<IPlaceService>();

            List<Place> placeList = new List<Place>();

            ResponseModel<List<PlaceViewModel>> response = null;

            try {
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

                response = new ResponseModel<List<PlaceViewModel>>(true, "Here are places that you want!", null, result);
            } catch (Exception) {
                response = ResponseModel<List<PlaceViewModel>>.CreateErrorResponse("Can not find result",systemError);
            }
            return Json(response);
        }
    }
}
