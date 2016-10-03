using SkyWeb.DatVM.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class SSNController : BaseController
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult UserProfile()
        {
            return View();
        }
    }
}