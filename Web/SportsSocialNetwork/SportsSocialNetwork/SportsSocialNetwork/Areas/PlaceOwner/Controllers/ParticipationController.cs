using SkyWeb.DatVM.Mvc;
using SportsSocialNetwork.Models.Entities.Services;
using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.PlaceOwner.Controllers
{
    public class ParticipationController : BaseController
    {
        // GET: PlaceOwner/Participation
        public ActionResult Index(int? id)
        {
            ViewBag.evenId = id;
            return View();
        }

        public ActionResult IndexList(JQueryDataTableParamModel request, int? id)
        {
            var service = this.Service<IParticipationService>();
            var totalRecord = 0;
            var count = 1;

            var results = service.GetPaticipation(request, out totalRecord)
                .AsEnumerable()
                .Where(x => x.EventId == id || id == null).Select(a => new IConvertible[] {
                        count++,
                        a.Event.Name,
                        a.AspNetUser.FullName,
                        a.Type,
                        a.TeamName,
                        a.Id,
                        a.AspNetUser.Id,
                        a.Event.Id
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
            var model = new ParticipationDetailViewModel();
            var service = this.Service<IParticipationService>();
            var participation = service.Get(id);
            if (participation == null)
            {
                return this.IdNotFound();
            }
            else
            {
                model = Mapper.Map<ParticipationDetailViewModel>(participation);
            }
            return this.PartialView(model);
        }

        [HttpPost]
        public ActionResult Deactive(int id)
        {
            var result = new AjaxOperationResult();
            var service = this.Service<IParticipationService>();
            var participation = service.Get(id);
            if (participation == null)
            {
                return this.IdNotFound();
            }
            else
            {
                try
                {
                    service.Delete(participation);
                    result.Succeed = true;
                }
                catch (Exception e)
                {
                    result.Succeed = false;
                }
            }
            return Json(result);
        }
    }
}