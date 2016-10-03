using SkyWeb.DatVM.Data;
using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Areas.Admin.Models;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Enumerable;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Admin.Controllers
{
    public class PlaceController : BaseController
    {
        // GET: Admin/Place
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request)
        {

            var service = this.Service<IPlaceService>();
            var totalRecord = 0;
            var count = 1;

            var results = service.GetPlaces(request, out totalRecord)
                .AsEnumerable()
                .Select(a => new IConvertible[] {
                        count++,
                        a.Name,
                        a.Address,
                        a.PhoneNumber,
                        a.AspNetUser.FullName,
                        a.Status,
                        a.Id

                }).ToArray();

            var model = new
            {
                draw = request.sEcho,
                data = results,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord
            };
            return Json(model);
        }

        public ActionResult Detail(int id)
        {
            var model = new PlaceDetailViewModel();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }else
            {
                model = Mapper.Map<PlaceDetailViewModel>(place);
                model.CreateAddressString();
            }
            return this.PartialView(model);
        }
        public ActionResult Update(int id)
        {
            
            return this.PartialView();
        }

        [HttpPost]
        public ActionResult RejectPlace(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    place.Approve = false;
                    place.Status = (int)PlaceStatus.Unapproved;
                    service.Update(place);
                    result.Succeed = true;
                }
                catch (Exception)
                {
                    result.Succeed = false;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult ApprovePlace(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    place.Approve = true;
                    place.Status = (int)PlaceStatus.Active;
                    service.Update(place);
                    result.Succeed = true;
                } catch (Exception)
                {
                    result.Succeed = false;
                }
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IPlaceService>();
            var place = service.Get(id);
            if (place == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    service.Deactivate(place);
                    result.Succeed = true;
                }
                catch (Exception)
                {
                    result.Succeed = false;
                }
            }
            return Json(result);
        }
    }
}