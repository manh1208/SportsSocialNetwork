using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    public class PlaceController : Controller
    {
        // GET: PlaceOwner/Place
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PlaceDetail(int? id)
        {
            var _placeService = this.Service<IPlaceService>();
            Place place = _placeService.FirstOrDefault(p => p.Id == id.Value);

            return View(place);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult savePlace(Place place)
        {
            var _placeService = this.Service<IPlaceService>();
            _placeService.savePlace(place);
            return RedirectToAction("Index");
        }

        public string deletePlace(int id)
        {
            var _placeService = this.Service<IPlaceService>();
            Place place = _placeService.FirstOrDefaultActive(p => p.Id == id);
            if (place != null)
            {
                _placeService.Deactivate(place);
                return "success";
            }
            return "false";
        }
    }
}