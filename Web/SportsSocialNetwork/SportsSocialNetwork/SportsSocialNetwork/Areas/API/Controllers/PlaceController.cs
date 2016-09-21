using Newtonsoft.Json;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.API.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.API.Controllers
{
    public class PlaceController : BaseController
    {
        

        public PlaceController(IPlaceService placeService)
        {
           
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult ShowAllPlaces() {
            var service = this.Service<IPlaceService>();
            List<Place> placeList = service.GetAll().ToList();
            ResponseModel<List<Place>> response = new ResponseModel<List<Place>>(true, "Load Place List Successfully", null, placeList);
            var result = JsonConvert.SerializeObject(response, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            //Mapper.Map<PlaceViewModel>
            String testString = result;

            return Json(testString,JsonRequestBehavior.AllowGet);
        }
    }
}
