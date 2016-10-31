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

namespace SportsSocialNetwork.Areas.Api.Controllers
{
    public class SportController : BaseController
    {
        private String systemError = "Đã có lỗi xảy ra!";

        [HttpPost]
        public ActionResult GetAllSport()
        {
            var service = this.Service<ISportService>();

            ResponseModel<List<SportViewModel>> response = null;

            try {
                List<Sport> sportList = service.GetActive().ToList();

                List<SportViewModel> result = Mapper.Map<List<SportViewModel>>(sportList);

                response = new ResponseModel<List<SportViewModel>>(true, "Danh sách các môn thể thao", null, result);

            } catch (Exception) {
                response = ResponseModel<List<SportViewModel>>.CreateErrorResponse("Không thể tải danh sách", systemError);
            }
            return Json(response);
        }
    }
}