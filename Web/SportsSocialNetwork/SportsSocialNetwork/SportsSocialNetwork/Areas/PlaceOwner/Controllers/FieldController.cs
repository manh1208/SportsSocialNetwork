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

        public ActionResult CreateField()
        {
            var _fieldTypeService = this.Service<IFieldTypeService>();
            List<FieldType> listFieldType = _fieldTypeService.Get().ToList();
            IOrderedEnumerable<SelectListItem> selectListFieldType = new List<SelectListItem>().OrderBy(d => d.Value);

            foreach (var item in listFieldType)
            {
                selectListFieldType.ToList().Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.selectListFieldType = selectListFieldType;
            return View();
        }
        
    }
}