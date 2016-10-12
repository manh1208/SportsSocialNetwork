using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models;
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
            ResponseModel<List<FieldDetailViewModel>> response = null;

            var service = this.Service<IFieldService>();

            List<Field> fieldList = null;

            try
            {

                fieldList = service.GetFieldList(placeId).ToList<Field>();

                List<FieldDetailViewModel> result = Mapper.Map<List<FieldDetailViewModel>>(fieldList);

                foreach (var field in fieldList)
                {
                    FieldImage image = field.FieldImages.FirstOrDefault();
                    if (image != null)
                    {
                        foreach (var overalField in result)
                        {
                            if (image.FieldId == overalField.Id)
                            {
                                overalField.Avatar = image.Image;
                            }
                        }
                    }
                }

                response = new ResponseModel<List<FieldDetailViewModel>>(true, "Field list loaded successfully", null, result);

            }
            catch (Exception)
            {

                response = ResponseModel<List<FieldDetailViewModel>>.CreateErrorResponse("Field list failed to load", systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ShowFieldDetail(int id) {
            ResponseModel<FieldDetailViewModel> response = null;

            var service = this.Service<IFieldService>();

            try {
                Field field = service.GetFieldInfo(id);
                if (field != null)
                {
                    FieldDetailViewModel result = Mapper.Map<FieldDetailViewModel>(field);

                    response = new ResponseModel<FieldDetailViewModel>(true, "Field detail loaded successfully", null, result);
                }
                else {
                    response = ResponseModel<FieldDetailViewModel>.CreateErrorResponse("Field detail has failed to load!", systemError);
                }

            } catch (Exception) {
                response = ResponseModel<FieldDetailViewModel>.CreateErrorResponse("Field detail has failed to load!",systemError);
            }

            return Json(response);
        }

        public ActionResult ChangeFieldStatus(int id, int status) {
            ResponseModel<FieldViewModel> response = null;

            var service = this.Service<IFieldService>();

            try {
                Field field = service.ChangeFieldStatus(id, status);

                if (field != null)
                {
                    FieldViewModel result = Mapper.Map<FieldViewModel>(field);

                    response = new ResponseModel<FieldViewModel>(true, "Field status changed successfully", null, result);
                }
                else {
                    response = ResponseModel<FieldViewModel>.CreateErrorResponse("Field status has failed to change");
                }
                
            } catch (Exception) {
                response = ResponseModel<FieldViewModel>.CreateErrorResponse("Field status has failed to change");
            }

            return Json(response);
        }
    }
}