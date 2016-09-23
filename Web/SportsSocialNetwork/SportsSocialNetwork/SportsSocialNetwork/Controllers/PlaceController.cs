using SportsSocialNetwork.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class PlaceController : Controller
    {
        // GET: Place
        private readonly IPlaceService _placeService;
        private readonly ISportService _sportService;

        public PlaceController(IPlaceService placeService, ISportService sportService)
        {
            _placeService = placeService;
            _sportService = sportService;
        }
        public ActionResult Index()
        {
            var sportList = _sportService.getAllSport();
            ViewBag.sportList = sportList;
            return View();
        }

        public ActionResult GetData(JQueryDataTableParamModel param, int sportId)
        {
            /*if (sportId != 0)
            {

            }*/
            var placeList = _placeService.getAllPlace();
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
                c.PlaceImages.First().Image,
                c.Name,
                c.Description,
                c.Address,
                //c.Fields.First().FieldType.,
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