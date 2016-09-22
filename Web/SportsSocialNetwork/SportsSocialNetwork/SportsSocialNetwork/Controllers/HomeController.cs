using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var service = this.Service<IAspNetUserService>();
            var users = service.Get().ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public JsonResult VietNam()
        {
            Country vietnam = AddressUtil.GetINSTANCE().GetCountry(Server.MapPath(AddressUtil.PATH));
            return Json(vietnam, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}