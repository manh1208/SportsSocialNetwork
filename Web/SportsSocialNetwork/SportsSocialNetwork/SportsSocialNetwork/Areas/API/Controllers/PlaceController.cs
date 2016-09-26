using Newtonsoft.Json;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.API.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
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
        public ActionResult ShowAllPlaces()
        {
            ResponseModel<List<PlaceViewModel>> response = null;

            var service = this.Service<IPlaceService>();

            List<Place> placeList = null;

            try
            {
                placeList = service.GetAll().ToList();
            }
            catch (Exception e)
            {
                response = ResponseModel<List<PlaceViewModel>>.CreateErrorResponse("Place List failed to load!", systemError);
                return Json(response, JsonRequestBehavior.AllowGet);
            }


            List<PlaceViewModel> viewmodel = Mapper.Map<List<PlaceViewModel>>(placeList);
            response = new ResponseModel<List<PlaceViewModel>>(true, "Load Place List Successfully", null, viewmodel);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetPlaceInfo(int id)
        {
            ResponseModel<PlaceViewModel> response = null;
            var service = this.Service<IPlaceService>();

            try
            {
                Place place = service.GetPlaceById(id);
                PlaceViewModel result = Mapper.Map<PlaceViewModel>(place);
                response = new ResponseModel<PlaceViewModel>(true, "Place info Loaded", null, result);
            }
            catch (Exception e)
            {
                response = ResponseModel<PlaceViewModel>.CreateErrorResponse("Place info failed to load!", systemError);
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
                PlaceViewModel result = Mapper.Map<PlaceViewModel>(place);
                response = new ResponseModel<PlaceViewModel>(true, "Your Place's status has been updated!", null, result);
            }
            catch (Exception e)
            {
                response = ResponseModel<PlaceViewModel>.CreateErrorResponse("Your Place's status has NOT been updated!", systemError);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
