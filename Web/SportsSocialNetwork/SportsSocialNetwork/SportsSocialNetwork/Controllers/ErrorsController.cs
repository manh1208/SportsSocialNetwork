using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult PageNotFound()
        {
            return View();
        }

       
        public ActionResult NoPermission()
        {
            return View();
        }

        
        public ActionResult DataError()
        {
            return View();
        }

        public ActionResult SearchResultNotFound()
        {
            return View();
        }
    }
}