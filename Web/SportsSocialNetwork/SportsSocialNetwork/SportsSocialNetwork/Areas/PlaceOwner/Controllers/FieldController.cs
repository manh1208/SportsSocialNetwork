using SportsSocialNetwork.Models.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.ViewModels;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities;
using System.Web.Routing;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.Utilities;
using SportsSocialNetwork.Models.Identity;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    [MyAuthorize(Roles = "Chủ sân")]

    public class FieldController : Controller
    {
        // GET: PlaceOwner/Field
        public ActionResult Index(int? id)
        {
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            Place place = _placeService.FirstOrDefaultActive(p => p.Id == id.Value);
            List<Field> fields = _fieldService.GetActive(f => f.PlaceId == id.Value).ToList();
            ViewBag.fields = fields;
            return View(place);
        }

        public ActionResult FieldList(int? id)
        {
            var _placeService = this.Service<IPlaceService>();
            var _fieldService = this.Service<IFieldService>();
            Place place = _placeService.FirstOrDefaultActive(p => p.Id == id.Value);
            List<Field> fields = _fieldService.GetActive(f => f.PlaceId == id.Value).ToList();
            ViewBag.fields = fields;
            return View(place);
        }

        public ActionResult CreateField(int? id)
        {
            var _fieldTypeService = this.Service<IFieldTypeService>();
            var _placeService = this.Service<IPlaceService>();
            List<FieldType> listFieldType = _fieldTypeService.GetActive().ToList();
            List<SelectListItem> selectListFieldType = new List<SelectListItem>();
            selectListFieldType.Add(new SelectListItem { Text = "", Value = "" });

            foreach (var item in listFieldType)
            {
                selectListFieldType.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            Place curPlace = _placeService.FirstOrDefault(p => p.Id == id.Value);
            ViewBag.curPlace = curPlace;
            ViewBag.selectListFieldType = selectListFieldType;
            return View();
        }

        public ActionResult FieldDetail(int? id)
        {
            var _fieldService = this.Service<IFieldService>();
            var _fieldImageService = this.Service<IFieldImageService>();
            var _timeBlockService = this.Service<ITimeBlockService>();
            var _fieldTypeService = this.Service<IFieldTypeService>();
            var _placeService = this.Service<IPlaceService>();

            Field field = _fieldService.FirstOrDefaultActive(f => f.Id == id.Value);
            List<FieldImage> fieldImages = _fieldImageService.Get(i => i.FieldId == id.Value).ToList();
            List<TimeBlock> timeBlocks = _timeBlockService.Get(t => t.FieldId == id.Value).ToList();
            List<FieldType> listFieldType = _fieldTypeService.Get().ToList();
            List<SelectListItem> selectListFieldType = new List<SelectListItem>();
            List<SelectListItem> statuss = new List<SelectListItem>();
            statuss.Add(new SelectListItem { Text = Utils.GetEnumDescription(PlaceStatus.Active), Value = Convert.ToString((int)PlaceStatus.Active) });
            statuss.Add(new SelectListItem { Text = Utils.GetEnumDescription(PlaceStatus.Repairing), Value = Convert.ToString((int)PlaceStatus.Repairing) });
            selectListFieldType.Add(new SelectListItem { Text = "", Value = "" });

            foreach (var item in listFieldType)
            {
                selectListFieldType.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            Place curPlace = _placeService.FirstOrDefault(p => p.Id == field.PlaceId);
            ViewBag.curPlace = curPlace;
            ViewBag.selectListFieldType = selectListFieldType;
            ViewBag.fieldImages = fieldImages;
            ViewBag.timeBlocks = timeBlocks;
            ViewBag.statusList = statuss;

            return View(field);
        }

        public ActionResult FieldInfoModal(int id)
        {
            var _fieldService = this.Service<IFieldService>();
            Field field = _fieldService.FirstOrDefaultActive(f => f.Id == id);
            return this.PartialView(field);
        }

        [HttpPost]
        public ActionResult createField(Field field, List<string> timeSlot, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            //field.PlaceId = 1009;
            var _fieldService = this.Service<IFieldService>();
            var _timeBlockService = this.Service<ITimeBlockService>();

            _fieldService.saveField(field);

            if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
            {
                var _fieldImageService = this.Service<IFieldImageService>();
                _fieldImageService.saveImage(field.Id, uploadImages);
            }

            _timeBlockService.saveTimeBLock(field.Id, timeSlot);

            return RedirectToAction("FieldList", new RouteValueDictionary(
                new { controller = "Field", action = "FieldList", id = field.PlaceId }));
        }

        [HttpPost]
        public ActionResult updateField(Field field, List<string> timeSlot, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            var _fieldService = this.Service<IFieldService>();
            var _timeBlockService = this.Service<ITimeBlockService>();

            _fieldService.saveField(field);

            if (uploadImages.ToList()[0] != null && uploadImages.ToList().Count > 0)
            {
                var _fieldImageService = this.Service<IFieldImageService>();
                _fieldImageService.saveImage(field.Id, uploadImages);
            }

            _timeBlockService.saveTimeBLock(field.Id, timeSlot);

            return RedirectToAction("FieldDetail", new RouteValueDictionary(
                new { controller = "Field", action = "FieldDetail", id = field.Id }));
        }

        [HttpPost]
        public ActionResult deleteTimeBlock(string timeblock)
        {
            var result = new AjaxOperationResult();
            var _timeBlockService = this.Service<ITimeBlockService>();
            if (_timeBlockService.deleteTimeBlock(timeblock) == true)
            {
                result.Succeed = true;
            }
            else
            {
                result.Succeed = false;
            }
            return Json(result);
        }

        [HttpPost]
        public string deleteImage(int id)
        {
            var _fieldImageService = this.Service<IFieldImageService>();
            FieldImage fieldImage = _fieldImageService.FirstOrDefaultActive(i => i.Id == id);
            if (fieldImage != null)
            {
                _fieldImageService.Delete(fieldImage);
                return "success";
            }
            return "false";
        }

        [HttpPost]
        public string deleteField(int id)
        {
            var _fieldService = this.Service<IFieldService>();
            Field field = _fieldService.FirstOrDefaultActive(f => f.Id == id);
            if (field != null)
            {
                _fieldService.Deactivate(field);
                return "success";
            }
            return "false";
        }

    }
}