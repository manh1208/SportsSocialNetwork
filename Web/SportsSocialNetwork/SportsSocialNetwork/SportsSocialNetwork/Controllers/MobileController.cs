using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class MobileController : BaseController
    {
        // GET: Mobile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewsDetail(int id)
        {
            var service = this.Service<INewsService>();
            News news = service.GetNewsById(id);
            NewsViewModel model = Mapper.Map<NewsViewModel>(news);
            return View(model);
        }
    }
}