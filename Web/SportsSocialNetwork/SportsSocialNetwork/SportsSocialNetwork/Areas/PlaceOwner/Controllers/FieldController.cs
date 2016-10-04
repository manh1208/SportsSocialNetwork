using SportsSocialNetwork.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.ViewModels;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    public class FieldController : Controller
    {
        // GET: PlaceOwner/Field
        public ActionResult Index()
        {
            return View();
        }

        
    }
}