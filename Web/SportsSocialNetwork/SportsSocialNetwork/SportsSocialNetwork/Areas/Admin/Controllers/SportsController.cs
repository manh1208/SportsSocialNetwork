using SkyWeb.DatVM.Mvc;
using SkyWeb.DatVM.Mvc.Autofac;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.Entities;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SportsSocialNetwork.Models.Identity;

namespace SportsSocialNetwork.Areas.Admin.Controllers
{
    [MyAuthorize(Roles = IdentityMultipleRoles.Admin)]
    public class SportsController : BaseController
    {
        // GET: Admin/Sports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexList (JQueryDataTableParamModel request)
        {
            var service = this.Service<ISportService>();
            var totalRecord = 0;
            var count = 1;

            var results = service.GetSport(request, out totalRecord)
                .AsEnumerable()
                .Select(a => new IConvertible[] {
                        count++,
                        a.Name,                       
                        a.Description,
                        a.Id,
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
            var model = new SportViewModel();
            var service = this.Service<ISportService>();
            var sport = service.Get(id);
            if (sport == null)
            {
                return this.IdNotFound();
            }
            else
            {
                model = Mapper.Map<SportViewModel>(sport);
                //model.
            }
            return this.PartialView(model);
        }

        [HttpGet]
         public ActionResult Create()
        {
            Sport sport = new Sport();
            return this.PartialView(sport);
        }

        [HttpPost]
        public ActionResult Create(Sport sport)
        {
            var result = new AjaxOperationResult();
            try
            {
                var service = this.Service<ISportService>();             
                service.Create(sport);
                service.Save();
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.Succeed = false;
            }
            return Json(result);
        }



        public ActionResult Update(int id)
        {
            var service = this.Service<ISportService>();
            Sport sport = service.Get(id);
            SportViewModel updatedSport;
            if(sport == null)
            {
                return this.IdNotFound();
            }
            else
            {
                updatedSport = Mapper.Map<SportViewModel>(sport);
            }
            return this.PartialView(updatedSport);
        }

        [HttpPost]
        public ActionResult Update(SportViewModel model)
        {
            var result = new AjaxOperationResult();
            try
            {
                var service = this.Service<ISportService>();
                Sport sport = service.Get(model.Id);
                sport.Name = model.Name;
                sport.Description = model.Description;
                service.Update(sport);
                service.Save();     
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.Succeed = false;
            }
            return Json(result);
        }
               
        public ActionResult Deactive(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<ISportService>();
            var sport = service.Get(id);
            if (sport == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    service.Deactivate(sport);
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