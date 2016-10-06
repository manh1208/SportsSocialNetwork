using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.Entities.Services;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.ViewModels;
using SkyWeb.DatVM.Mvc;

namespace SportsSocialNetwork.Controllers
{
    public class FieldController : BaseController
    {
        // GET: Field
        public ActionResult Index(int? id)
        {
            var _fieldService = this.Service<IFieldService>();
            var _fieldImageService = this.Service<IFieldImageService>();
            var entity = _fieldService.FirstOrDefaultActive(p => p.Id == id.Value);
            var fields = new FieldViewModel(entity);
            List<FieldImage> fieldImages = _fieldImageService.Get(p => p.FieldId == id.Value).ToList();
            if (fieldImages.Count > 0)
            {
                ViewBag.fieldImages = fieldImages;
            }
            return View(fields);
        }
        public ActionResult GetSchedule(int? id)
        {
            //nen tao 1 bang quan ly gio san rieng, gio dung tam bang order
            var _orderService = this.Service<IOrderService>();
            List<Order> orders = _orderService.Get(p => p.FieldId == id).ToList();
            List<OrderViewModel> orderList = Mapper.Map<List<OrderViewModel>>(orders);
            return Json(orderList.Select(f => new { title = "Đã được đặt",start=f.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),end=f.EndTime.ToString("yyyy-MM-ddTHH:mm:ss") }), JsonRequestBehavior.AllowGet );
        }

        
    }
}