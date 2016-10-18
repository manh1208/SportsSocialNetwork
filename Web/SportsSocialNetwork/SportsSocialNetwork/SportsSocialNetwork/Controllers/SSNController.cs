using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.SSN)]
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