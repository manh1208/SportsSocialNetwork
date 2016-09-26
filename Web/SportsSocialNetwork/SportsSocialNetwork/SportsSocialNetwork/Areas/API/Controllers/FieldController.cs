using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.API.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.API.Controllers
{
    public class FieldController : BaseController
    {
        private String systemError = "An error has occured!";

        [HttpPost]
        public ActionResult ShowFieldList(int placeId)
        {
            ResponseModel<List<FieldViewModel>> response = null;

            var service = this.Service<IFieldService>();

            List<Field> fieldList = null;

            try
            {

                fieldList = service.GetFieldList(placeId).ToList<Field>();

            }
            catch (Exception e)
            {

                response = ResponseModel<List<FieldViewModel>>.CreateErrorResponse("Field list failed to load", systemError);
                return Json(response);
            }

            List<FieldViewModel> result = Mapper.Map<List<FieldViewModel>>(fieldList);

            response = new ResponseModel<List<FieldViewModel>>(true, "Field list loaded successfully", null, result);

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowFieldDetail(int id) {
            ResponseModel<FieldViewModel> response = null;

            var service = this.Service<IFieldService>();

            try {
                Field field = service.GetFieldInfo(id);

                FieldViewModel result = Mapper.Map<FieldViewModel>(field);

                response = new ResponseModel<FieldViewModel>(true, "Field detail loaded successfully", null, result);

            } catch (Exception e) {
                response = ResponseModel<FieldViewModel>.CreateErrorResponse("Field detail has failed to load!",systemError);
            }

            return Json(response);
        }

        public ActionResult ChangeFieldStatus(int id, int status) {
            ResponseModel<FieldViewModel> response = null;

            var service = this.Service<IFieldService>();

            try {
                Field field = service.ChangeFieldStatus(id, status);

                FieldViewModel result = Mapper.Map<FieldViewModel>(field);

                response = new ResponseModel<FieldViewModel>(true, "Field status changed successfully", null, result);
            } catch (Exception e) {
                response = ResponseModel<FieldViewModel>.CreateErrorResponse("Field status has failed to change");
            }

            return Json(response);
        }
    }
}