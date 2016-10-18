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
        private String systemError = "Đã có lỗi xảy ra!";

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

                response = new ResponseModel<List<FieldDetailViewModel>>(true, "Danh sách sân:", null, result);

            }
            catch (Exception)
            {

                response = ResponseModel<List<FieldDetailViewModel>>.CreateErrorResponse("Thất bại khi tải danh sách sân!", systemError);
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

                    response = new ResponseModel<FieldDetailViewModel>(true, "Chi tiết sân:", null, result);
                }
                else {
                    response = ResponseModel<FieldDetailViewModel>.CreateErrorResponse("Tải chi tiết sân thất bại!", systemError);
                }

            } catch (Exception) {
                response = ResponseModel<FieldDetailViewModel>.CreateErrorResponse("Tải chi tiết sân thất bại!", systemError);
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

                    response = new ResponseModel<FieldViewModel>(true, "Trạng thái sân đã được cập nhật thành công", null, result);
                }
                else {
                    response = ResponseModel<FieldViewModel>.CreateErrorResponse("Cập nhật trạng thái sân thất bại",systemError);
                }
                
            } catch (Exception) {
                response = ResponseModel<FieldViewModel>.CreateErrorResponse("Cập nhật trạng thái sân thất bại",systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult GetFieldTypeByPlaceId(int placeId) {
            var service = this.Service<IFieldService>();

            ResponseModel<List<FieldTypeViewModel>> response = null;

            try {
                List<FieldType> typeList = service.GetFieldTypeByPlaceId(placeId);

                List<FieldTypeViewModel> result = Mapper.Map<List<FieldTypeViewModel>>(typeList);

                response = new ResponseModel<List<FieldTypeViewModel>>(true, "Danh sách loại sân:", null, result);
            } catch (Exception) {
                response = ResponseModel<List<FieldTypeViewModel>>.CreateErrorResponse("Không thể tải loại sân",systemError);
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult GetFieldByFieldTypeId(int placeId, int fieldTypeId)
        {
            var service = this.Service<IFieldService>();

            ResponseModel<List<FieldDetailViewModel>> response = null;

            try {
                List<Field> fieldList = service.FindAllFieldsByFieldType(placeId, fieldTypeId).ToList();

                List<FieldDetailViewModel> result = Mapper.Map<List<FieldDetailViewModel>>(fieldList);

                response = new ResponseModel<List<FieldDetailViewModel>>(true, "Kết quả tìm kiếm:", null, result);
            } catch (Exception) {
                response = ResponseModel<List<FieldDetailViewModel>>.CreateErrorResponse("Tìm kiếm thất bại",systemError);
            }
            return Json(response);
        }
    }
}